# 🚀 Blog System API

A modern and scalable Blog Backend API built with **ASP.NET Core** and **Clean Architecture** principles.

This project is designed to demonstrate real-world backend development practices including Authentication, Authorization, Validation, Domain Separation, and Token Management.

---

## ✨ Features

### 🔐 Authentication & Authorization

* User Registration
* User Login
* JWT Authentication
* Refresh Token Authentication
* Secure Token Rotation
* Protected Endpoints

### 📝 Blog Management

* Create Post
* Get Post
* Update Post
* Delete Post
* Manage Published Posts

### 🏷️ Tags & Categories

* Add Hashtags to Posts
* Organize Content

### 💬 Comment System

* Create Comment
* Retrieve Comments
* Comment Management

### 👍 Post Interactions

* Like Posts
* Dislike Posts
* Update User Reactions

### 🛡️ Validation

* Request Validation using FluentValidation
* Clean Error Handling
* Consistent API Responses

---

## 🏗️ Architecture

This project follows the **Clean Architecture** pattern.

```text
BlogSystem
│
├── Presentation Layer (API)
│
├── Application Layer
│   ├── UseCases
│   ├── DTOs
│   ├── Interfaces
│
├── Domain Layer
│   ├── Entities
│   ├── Enums
│   ├── Contracts
│
├── Infrastructure Layer
│   ├── EF Core
│   ├── Repositories
│   ├── Services
│
└── Persistence
    └── SQL Server
```

The architecture separates business rules from external concerns, making the application easier to maintain, test, and extend.

---

## 🛠️ Technologies Used

### Backend

* ASP.NET Core Web API
* C#
* .NET

### Database

* SQL Server
* Entity Framework Core
* LINQ

### Security

* JWT Authentication
* Refresh Token
* Claims-Based Authentication
* Password Hashing

### Validation

* FluentValidation

### Design Patterns

* Clean Architecture
* Repository Pattern
* Dependency Injection
* Result Pattern
* Unit of Work (Planned)

---

## 🔄 Authentication Flow

```text
Login
   │
   ▼
Access Token + Refresh Token
   │
   ▼
Access Token Expired
   │
   ▼
Refresh Token Endpoint
   │
   ▼
New Access Token + New Refresh Token
```

The project implements Refresh Token Rotation for improved security.

---

## 📦 API Capabilities

### Authentication

* Register User
* Login User
* Refresh Token

### Posts

* Create Post
* Get All Posts
* Get Post By Id
* Update Post
* Delete Post

### Comments

* Add Comment
* Get Comments

### Tags

* Add Tag To Post

### Reactions

* Like Post
* Dislike Post

---

## 🎯 Project Goals

This project was created to practice and demonstrate:

* Clean Architecture
* Secure Authentication
* Entity Framework Core
* Repository Pattern
* API Design
* Validation
* Database Design
* Backend Best Practices

---

## 🚀 Future Improvements

* Role-Based Authorization
* Redis Caching
* Pagination
* Search & Filtering
* SignalR Notifications
* Docker Support
* Unit Testing
* Integration Testing

---

## 📚 Learning Focus

This project is not just a CRUD application.

It focuses on building production-oriented backend concepts such as:

* Authentication & Security
* Refresh Token Flow
* Layered Architecture
* Domain Separation
* Scalable API Design

---

### ⭐ If you find this project useful, consider giving it a star.
