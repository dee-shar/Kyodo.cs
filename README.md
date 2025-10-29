# Kyodo.cs
Mobile-API for Kyodo an social network where you can meet new friends and join circles with likeminded people, share and create content

## Example
```cs
using KyodoApi;

namespace Application
{
    internal class Program
    {
        static async Task Main()
        {
            var api = new Kyodo();
            await api.Login("example@gmail.com", "password");
            string accountInfo = await api.GetAccountInfo();
            Console.WriteLine(accountInfo);
        }
    }
}
```
