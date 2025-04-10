# 💸 Moneygram API (.NET 8)

A secure and scalable .NET 8 Web API for processing MoneyGram transactions via Agent Connect v15.12. Available is sends, kyc check, kyc registration, kyc suggestions based on past data, beneficiary management for 2nd time customers, health checks, and audit logging. IP whitelisting & JWT authentication are also included for security.

---

## 📁 Project Structure

```bash
moneygram-api/
│
├── Controllers/               # API controllers (entry points for HTTP requests)
├── DTOs/                      # Data Transfer Objects used between layers
├── Enums/                     # Enum definitions used across the app
├── Exceptions/                # Custom exception classes and handlers
├── Middleware/                # Middleware for logging, error handling, etc.
├── Models/                    # Database models / entities
├── Services/                  # Core business logic and service interfaces
├── Settings/                  # Custom strongly-typed settings classes
├── Utilities/                 # Helper methods and common utilities
├── HealthChecks/              # Health check endpoints and logic
├── Migrations/                # EF Core database migrations
├── Data/                      # DbContext and seeders
│
├── appsettings.json           # Base app configuration
├── appsettings.Development.json  # Dev-specific overrides (ignored in Git)
├── moneygram-api.csproj       # Project file
├── moneygram-api.sln          # Solution file
├── Program.cs                 # Entry point for the API
├── README.md                  # This file
├── test-keys/                 # Keys or test data (exclude sensitive info)
├── publish/                   # Output folder for published builds
└── moneygram-api.http         # HTTP request test file (VS Code/REST Client)
```

---

## ⚙️ Configuration

### appsettings.json

Contains core configuration like:

- Connection strings
- Logging settings
- API keys
- External service URLs

**EXCLUDED** sensitive settings — these are ignored via `.gitignore`.

---

## 🚀 Running the App

Make sure you have the [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed.

### Run locally:

```bash
dotnet run
```
---

## 🚀 Swagger
Once running, access Swagger UI at:

``` 
https://localhost:5001/swagger
```
---

## 📦 Publish
Publish to folder (for deployment):
```bash
dotnet publish -c Release -o ./publish
```
This will create a self-contained deployment in the `publish` folder.
You can then copy the contents of this folder to your server and run the application.

---

## 🛡️ Security
	•	Middleware handles structured exception logging
	•	Sensitive configs excluded via .gitignore
	•	Input validations via DTOs

---

## ✍️ Author
### Nick Dhliwayo
Your friendly Dev Jedi Master
[GitHub](https://github.com/nickattack97) @nickattack97