# 🚀 SmartCertify – Clean Architecture .NET 9 API

Welcome to the **SmartCertify Clean Architecture .NET 9 API** project!

This repository demonstrates how to build a **scalable, enterprise-grade Web API** using **.NET 9** and **Clean Architecture principles**. It is part of the SmartCertify full-stack platform for managing online courses, assessments, and certifications.

---

# 📌 Project Overview

**SmartCertify** is an online course certification platform that allows users to:

* Browse and enroll in courses
* Take assessments
* Earn certifications
* Track learning progress
* Receive notifications
* Upload profile images securely

This project showcases modern full-stack and cloud development practices using .NET, Angular, and Azure.

---

# 🏗️ Clean Architecture – Visual Diagram

Below is the high-level architecture used in this solution:

```
                   ┌──────────────────────┐
                   │      Client Apps     │
                   │  (Angular / Mobile) │
                   └──────────┬──────────┘
                              │ HTTP / HTTPS
                              ▼
                   ┌──────────────────────┐
                   │   SmartCertify.API  │
                   │  Controllers        │
                   │  Middleware         │
                   │  Filters            │
                   └──────────┬──────────┘
                              │
                              ▼
                   ┌──────────────────────┐
                   │   Application Layer  │
                   │  Services / UseCases│
                   │  DTOs / Validators  │
                   │  Interfaces         │
                   └──────────┬──────────┘
                              │
                              ▼
                   ┌──────────────────────┐
                   │     Domain Layer     │
                   │  Entities           │
                   │  Enums              │
                   │  Business Rules     │
                   └──────────┬──────────┘
                              │
                              ▼
                   ┌──────────────────────┐
                   │ Infrastructure Layer │
                   │ EF Core / Repos     │
                   │ External Services   │
                   │ Azure Integrations  │
                   └──────────┬──────────┘
                              │
                              ▼
                   ┌──────────────────────┐
                   │      Database        │
                   │   SQL Server / Azure│
                   └──────────────────────┘
```

### Dependency Rule

```
API → Application → Domain
        ↓
 Infrastructure
```

* Domain has **no dependencies**
* Application depends only on Domain
* Infrastructure implements Application contracts
* API depends on Application

---

# 📁 Project Folder Structure

```
smartcertify-api-clean-architecture-dotnet9/
│
├── src/
│   ├── SmartCertify.API/
│   │   ├── Controllers/
│   │   ├── Middleware/
│   │   ├── Filters/
│   │   ├── Extensions/
│   │   └── Program.cs
│   │
│   ├── SmartCertify.Application/
│   │   ├── Interfaces/
│   │   ├── Services/
│   │   ├── DTOs/
│   │   ├── Features/
│   │   └── Validators/
│   │
│   ├── SmartCertify.Domain/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   └── Common/
│   │
│   ├── SmartCertify.Infrastructure/
│   │   ├── Data/
│   │   ├── Repositories/
│   │   ├── Configurations/
│   │   └── ExternalServices/
│   │
│   └── SmartCertify.BackgroundServices/
│
├── tests/
├── docker/
├── SmartCertify.sln
└── README.md
```

---

# ⚙️ Tech Stack

## Backend

* .NET 9 Web API
* Entity Framework Core
* Fluent Validation
* Background Services

## Database

* SQL Server / SQL Express
* Azure SQL Database

## API Documentation

* Swagger
* NSwag
* Scalar

## Cloud & DevOps

* Azure App Service
* Azure Storage (Blob)
* Azure Functions
* Azure AD B2C
* Azure Key Vault
* Azure Application Insights
* Azure DevOps CI/CD

## Frontend (Companion App)

* Angular 19 (Standalone Components)

---

# 🔌 API Endpoints Overview

| Module         | Method | Endpoint                       | Description       |
| -------------- | ------ | ------------------------------ | ----------------- |
| Courses        | GET    | `/api/courses`                 | Get all courses   |
| Courses        | GET    | `/api/courses/{id}`            | Get course by ID  |
| Courses        | POST   | `/api/courses`                 | Create course     |
| Courses        | PUT    | `/api/courses/{id}`            | Update course     |
| Courses        | PATCH  | `/api/courses/{id}`            | Partial update    |
| Courses        | DELETE | `/api/courses/{id}`            | Delete course     |
| Users          | GET    | `/api/users`                   | Get users         |
| Users          | GET    | `/api/users/{id}`              | Get user profile  |
| Certifications | GET    | `/api/certifications/{userId}` | User certificates |
| Notifications  | GET    | `/api/notifications`           | Get notifications |
| Files          | POST   | `/api/files/upload`            | Upload image      |
| Auth           | POST   | `/api/auth/login`              | Login (AD B2C)    |

📄 Full list available in Swagger.

---

# 🧪 Key Features

* Clean Architecture implementation
* EF Core database integration
* Full CRUD + PATCH endpoints
* Fluent Validation
* Global exception handling
* Role-based authorization
* Secure file uploads
* Background processing
* Email automation
* API monitoring

---

# 🗄️ Database Tools

Install:

* SQL Server / SQL Express
* SSMS
* Azure Data Studio

Used for DB design, queries, and cloud connectivity.

---

# ▶️ Getting Started

### Clone Repo

```bash
git clone https://github.com/learnsmartcoding/smartcertify-api-clean-architecture-dotnet9
cd smartcertify-api-clean-architecture-dotnet9
```

### Restore Packages

```bash
dotnet restore
```

### Run Project

```bash
dotnet run
```

Swagger:

```
http://localhost:5000/swagger
```

---

# 🐳 Docker Setup

## Dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SmartCertify.API.dll"]
```

## Build Image

```bash
docker build -t smartcertify-api .
```

## Run Container

```bash
docker run -d -p 8080:80 smartcertify-api
```

Swagger:

```
http://localhost:8080/swagger
```

---

## Docker Compose (Optional)

```yaml
version: '3.8'

services:
  api:
    build: .
    ports:
      - "8080:80"
    depends_on:
      - sqlserver

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      SA_PASSWORD: "Your_password123"
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"
```

---

# ☁️ Cloud Features

* Azure App Service deployment
* CI/CD with Azure DevOps
* App Insights monitoring
* Key Vault secrets
* AD B2C authentication
* Blob Storage uploads

---

# 🔐 Security

* Azure AD B2C login
* JWT token enrichment
* Role-based access
* SAS token file access
* Managed Identity

---

# 🌐 Live Demo

[https://smartcertify-web.azurewebsites.net/home](https://smartcertify-web.azurewebsites.net/home)

---

# 📚 Learning Outcomes

By exploring this repo, you will learn:

* Clean Architecture in .NET
* Enterprise API design
* Angular + .NET integration
* Azure cloud services
* CI/CD pipelines
* Secure authentication

---

# 🤝 Community

Telegram Community:
[https://t.me/LearnSmartCodingYTCommunity](https://t.me/LearnSmartCodingYTCommunity)

---

# ⭐ Support

If you find this project useful:

* Star ⭐ the repo
* Fork 🍴 it
* Share 📢 with others

---

# 📄 License

For educational purposes. Add a license if required.

---
