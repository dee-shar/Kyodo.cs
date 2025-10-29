using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text.Json;
using System.Net.Http.Json;

namespace KyodoApi
{
    public class Kyodo
    {
        private string apiToken;
        private readonly string deviceId;
        private readonly string signature;
        private readonly string signatureHash;
        private readonly HttpClient httpClient;
        private readonly string apiUrl = "https://api.kyodo.app/v1";
        private const string signatureKey = "9d93933f-7864-4872-96b2-9541ac03cf6c";
        public Kyodo()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "okhttp/4.9.2");
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            deviceId = GenerateDeviceId();
            signature = GenerateSignature();
            httpClient.DefaultRequestHeaders.Add("device-id", deviceId);
            httpClient.DefaultRequestHeaders.Add("x-Signature", signature);
        }

        private static string Base64UrlEncode(byte[] input) =>
            Convert.ToBase64String(input).TrimEnd('=').Replace('+', '-').Replace('/', '_');

        private static string GenerateDeviceId() => Guid.NewGuid().ToString("N")[..13];

        private static string GenerateSignature()
        {
            var header = Base64UrlEncode(
                Encoding.UTF8.GetBytes(
                    JsonSerializer.Serialize(new { typ = "JWT", alg = "HS256" })));
            var payload = Base64UrlEncode(
                Encoding.UTF8.GetBytes(
                    JsonSerializer.Serialize(
                        new { typeof_ = "xSig", expirationTime = DateTimeOffset.UtcNow.AddSeconds(10).ToUnixTimeSeconds() })));
            var signature = Base64UrlEncode(
                new HMACSHA256(
                    Encoding.ASCII.GetBytes(signatureKey)).ComputeHash(Encoding.ASCII.GetBytes($"{header}.{payload}")));
            return $"{header}.{payload}.{signature}";
        }

        // Дописать после того как протрезвею и проблеваюсь от энергетиков 
        private static string GenerateSignatureHash(object credentials) =>
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(credentials)))).ToLower();

        public async Task<string> Login(string email, string password)
        {
            var data = JsonContent.Create(new
            {
                type = 0,
                email = email,
                password = password
            });
            var response = await httpClient.PostAsync($"{apiUrl}/g/s/auth/login", data);
            var content = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(content);
            if (document.RootElement.TryGetProperty("apiToken", out var tokenElement))
            {
                apiToken = tokenElement.GetString();
                httpClient.DefaultRequestHeaders.Add("authorization", apiToken);
            }
            return content;
        }

        public async Task<string> GetJoinedCommunities(int limit = 100)
        {
            var response = await httpClient.GetAsync($"{apiUrl}/g/s/homefeed/joined?limit={limit}");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAccountInfo()
        {
            var response = await httpClient.GetAsync($"{apiUrl}/g/s/users/me");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetJoinedCircles()
        {
            var response = await httpClient.GetAsync($"{apiUrl}/g/s/accounts/joined-circles");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetUnreadChats()
        {
            var response = await httpClient.GetAsync($"{apiUrl}/g/s/chats/unread");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetUnreadCircles()
        {
            var response = await httpClient.GetAsync($"{apiUrl}/g/s/circles/unread");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetUnreadNotificationsCount()
        {
            var response = await httpClient.GetAsync($"{apiUrl}/g/s/notifications/unread-count");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SendEmailVerification()
        {
            var response = await httpClient.PostAsync($"{apiUrl}/g/s/auth/resend-verification-email", null);
            return await response.Content.ReadAsStringAsync();
        }
      
        public async Task<string> GetCircleInfo(string circleId)
        {
            var response = await httpClient.GetAsync($"{apiUrl}/{circleId}/s/circles");
            return await response.Content.ReadAsStringAsync();
        }

    }
}
