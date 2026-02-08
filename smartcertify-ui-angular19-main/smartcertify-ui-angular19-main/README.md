# SmartCertify – Clean Architecture .NET 9 API + Angular 19 + Azure Services

Welcome to the **SmartCertify Full-Stack Platform** 🚀

This repository demonstrates how to build a **production-grade online course certification system** using:

* **.NET 9 Web API**
* **Angular 19 (Standalone)**
* **Azure Cloud Services**
* **Clean Architecture principles**

The solution is designed to be scalable, secure, and cloud-ready, following enterprise development standards.

---

# 📌 Solution Overview

SmartCertify is an online learning and certification platform that enables:

* Course management
* User enrollment
* Assessments & certifications
* Secure authentication
* File & image storage
* Notifications & automation

It is implemented as a **full-stack cloud-native system**.

---

# 🏗️ Architecture

The backend follows **Clean Architecture** to ensure separation of concerns and maintainability.

## 🔷 Architecture Diagram

```
                ┌──────────────────────────┐
                │        Angular 19 UI      │
                │  (Standalone Components) │
                └─────────────┬────────────┘
                              │ HTTP / REST
                              ▼
┌──────────────────────────────────────────────────────────┐
│                 .NET 9 Web API (Presentation)             │
│ Controllers • Middleware • Filters • Swagger/NSwag       │
└───────────────────────┬──────────────────────────────────┘
                        │
                        ▼
┌──────────────────────────────────────────────────────────┐
│                 Application Layer                        │
│ Use Cases • DTOs • Interfaces • Validators               │
└───────────────────────┬──────────────────────────────────┘
                        │
                        ▼
┌──────────────────────────────────────────────────────────┐
│                   Domain Layer                           │
│ Entities • Enums • Business Rules • Aggregates           │
└───────────────────────┬──────────────────────────────────┘
                        │
                        ▼
┌──────────────────────────────────────────────────────────┐
│                Infrastructure Layer                      │
│ EF Core • Repositories • Azure Services • Storage        │
└──────────────────────────────────────────────────────────┘
```

---

# 📂 Project Folder Structure

```
SmartCertify.sln

├── SmartCertify.API              → Presentation Layer
│   ├── Controllers
│   ├── Middleware
│   ├── Filters
│   └── Program.cs

├── SmartCertify.Application      → Application Layer
│   ├── DTOs
│   ├── Interfaces
│   ├── Services
│   ├── Validators
│   └── Mappings

├── SmartCertify.Domain           → Core Domain
│   ├── Entities
│   ├── Enums
│   ├── ValueObjects
│   └── Constants

├── SmartCertify.Infrastructure   → External Implementations
│   ├── DbContext
│   ├── Configurations
│   ├── Repositories
│   ├── AzureStorage
│   └── Identity

├── SmartCertify.AngularUI        → Angular 19 Frontend
│   ├── app
│   ├── features
│   ├── shared
│   └── services
```

---

# ⚙️ Technology Stack

## Backend

* .NET 9 Web API
* Entity Framework Core
* SQL Server / Azure SQL
* Fluent Validation
* Background Services

## Frontend

* Angular 19
* Standalone Components
* Signals State Management
* Lazy Loading & Routing

## Cloud & DevOps

* Azure App Services
* Azure SQL Database
* Azure Storage (Blob)
* Azure Functions
* Azure AD B2C
* Azure Key Vault
* Azure Application Insights
* Azure DevOps CI/CD

## API Documentation

* NSwag
* Scalar
* OpenAPI

---

# 🔌 API Endpoint Overview

| Module  | Method | Endpoint          | Description      |
| ------- | ------ | ----------------- | ---------------- |
| Courses | GET    | /api/courses      | Get all courses  |
| Courses | GET    | /api/courses/{id} | Get course by ID |
| Courses | POST   | /api/courses      | Create course    |
| Courses | PUT    | /api/courses/{id} | Update course    |
| Courses | PATCH  | /api/courses/{id} | Partial update   |
| Courses | DELETE | /api/courses/{id} | Delete course    |
| Users   | GET    | /api/users        | Get users        |
| Users   | POST   | /api/users        | C                |
