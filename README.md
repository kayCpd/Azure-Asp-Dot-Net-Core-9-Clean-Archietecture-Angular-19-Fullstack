# 🚀 SmartCertify – Clean Architecture .NET 9 API

Welcome to the **SmartCertify Clean Architecture .NET 9 API** project.

This repository demonstrates how to build a **scalable, enterprise-grade Web API** using **.NET 9** and **Clean Architecture principles**. It forms the backend foundation of the SmartCertify platform for managing online courses, assessments, and certifications.

---

# 📌 Project Overview

SmartCertify is an online course certification system that enables users to:

* Browse and enroll in courses
* Take assessments
* Earn certifications
* Track learning progress
* Receive notifications
* Upload profile images securely

The solution showcases modern **API design, cloud integration, and secure application architecture**.

---

# 🏗️ Clean Architecture

The project follows **Clean Architecture** to ensure maintainability, scalability, and separation of concerns.

## 🔷 Architecture Diagram

```
                   ┌──────────────────────┐
                   │      Client Apps     │
                   │  (Angular / Mobile) │
                   └──────────┬──────────┘
                              │ HTTP / HTTPS
                              ▼
                   ┌──────────────────────┐
                   │   SmartCertify.API   │
                   │  Controllers         │
                   │  Middleware          │
                   │  Filters             │
                   └──────────┬──────────┘
                              │
                              ▼
                   ┌──────────────────────┐
                   │  Application Layer   │
                   │  Use Cases / Services│
                   │  DTOs / Validators   │
                   │  Interfaces          │
                   └─────────
```
