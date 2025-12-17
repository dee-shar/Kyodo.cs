# Kyodo.cs 📱

**Kyodo.cs** is a .NET client library for interacting with the **Kyodo** social network API.  
It allows you to authenticate, fetch user data, publish content, join circles, meet new people, and more — all directly from C# applications.

<p align="center">
  <img src="https://github.com/user-attachments/assets/083adfac-331f-430c-9acc-5874d982e048" width="400" alt="Kyodo logo" />
</p>

---

## 📖 Overview

**Kyodo.cs** is a lightweight C# client library designed to interface seamlessly with the Kyodo mobile backend.  
It handles the heavy lifting of mobile integration — including **HMACSHA256 signature generation**, device identification, and session management — allowing developers to build Kyodo-powered applications with just a few lines of code.

---
## 🚀 Features

- 🧑‍💻 Easy login & authentication  
- 🔍 Fetch user profile and account details  
- 💬 Create and share posts  
- 🤝 Explore circles and connect with people  
- 📱 Built for .NET Standard — works with Desktop, Mobile & Server apps  
- 💡 Simplifies working with the Kyodo Mobile API in C#  
- 🔐 Automatic generation of `x-Signature` and `device-id` headers  
- ⚡ Fully asynchronous (`async/await`)  
---

## 📦 Quick Start

```csharp
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

---

## 🛠 API Reference

| Category | Method | Description |
|--------|--------|-------------|
| **Authentication** | `Login()` | Authenticates the user and stores the API token |
| **User Profile** | `GetAccountInfo()` | Retrieves current user metadata and settings |
| **Circles** | `GetJoinedCircles()` | Returns a list of joined circles |
| **Messaging** | `GetUnreadChats()` | Fetches unread chat messages |
| **Discovery** | `GetJoinedCommunities()` | Accesses home feed and followed communities |

---

## ⚖️ License

This project is open-source.  
Please refer to the repository for specific licensing details.
