# 🚀 Blog System API

A modern and scalable Blog Backend API built with **ASP.NET Core** and **Clean Architecture** principles.

This project demonstrates real-world backend development practices including Authentication, Authorization, Validation, Domain Separation, Caching, and Containerization.

---

## ✨ Features

### 🔐 Authentication & Authorization

* User Registration
* User Login
* JWT Authentication
* Refresh Token Authentication
* Secure Token Rotation
* Protected Endpoints

---

### 📝 Blog Management

* Create Post
* Get Post
* Update Post
* Delete Post
* Manage Published Posts

---

### 🏷️ Tags & Categories

* Add Hashtags to Posts
* Organize Content

---

### 💬 Comment System

* Create Comment
* Retrieve Comments
* Comment Management

---

### 👍 Post Interactions

* Like Posts
* Dislike Posts
* Update User Reactions

---

### ⚡ Performance & Caching

* Redis Caching for Posts
* In-Memory Performance Optimization
* Reduced Database Load
* Cache-Aside Pattern Implementation

---

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
│
├── Domain Layer
│   ├── Entities
│   ├── Enums
│   ├── Interfaces
│   ├── Models
│
├── Infrastructure Layer
│   ├── EF Core
│   ├── Repositories
│   ├── Services
│   ├── Redis Caching
│
└── Persistence
    └── SQL Server
