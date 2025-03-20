# Task Management API

## Overview
This is a **Task Management API** built using **C# (.NET 9), ASP.NET Core Web API, and MSSQL**. It provides CRUD operations for managing user tasks, including task creation, retrieval, updating, and deletion. It also features JWT authentication, ensuring that tasks are securely linked to their respective owners.

## Features
- **JWT Authentication:** Secure API endpoints with user authentication.
- **User Task Management:** Tasks are linked to their respective owners.
- **Priority-Based Filtering:** Retrieve tasks based on priority levels.
- **Logging & Error Handling:** Logs errors and provides meaningful responses.
- **DTO-Based API Responses:** Uses **Data Transfer Objects (DTOs)** for structured responses.
- **Dependency Injection:** Implements **service interfaces** for better abstraction.

## Technologies Used
- **C# (.NET 9)**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server (MSSQL)**
- **JWT Authentication**
- **Dependency Injection (DI)**
- **Logging (ILogger)**
- **Swagger for API Documentation**
- **Task-Based Asynchronous Programming (Async/Await)**

## Installation & Setup

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/your-username/task-management-api.git
cd task-management-api
```

### 2️⃣ Configure the Database
Ensure **MSSQL Server** is running, then update the **connection string** in `appsettings.json`:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=TaskManagementDB;User Id=your_user;Password=your_password;"
}
```

### 3️⃣ Set Up JWT Authentication
Before running the API, configure authentication in `appsettings.json`:
```json
"JWT": {
    "SecretKey": "YourSecretKey",
    "Issuer": "TaskManagementAPI",
    "Audience": "TaskManagementClient"
}
```

### Configure JWT Authentication in `Program.cs`
```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
        };
    });

builder.Services.AddAuthorization();
```

### 4️⃣ Run Database Migrations
```bash
dotnet ef database update
```

### 5️⃣ Run the API
```bash
dotnet run
```

## API Documentation
The API documentation is available on Postman:
[View API Docs](https://documenter.getpostman.com/view/33708307/2sAYkBuNLL)

The API will be available at:
```http
http://localhost:5143/api/usertasks
```

## API Endpoints

### 🔒 User Authentication

#### ✅ Register a User
```http
POST /api/auth/register
Content-Type: application/json
```

**Response:**
```json
{
    "userId": "userId",
    "username": "username",
    "email": "your@email.com",
    "token": "eynZW1lbnRDbGllbnQifQ.-dLCSNLIwQSl98iDTMwwSmUaVI9Thqly-UaORnF729A",
    "expiresAt": "0001-01-01T00:00:00",
    "role": "User"
}
```

#### ✅ Login & Token Generation
```http
POST /api/auth/login
Content-Type: application/json
```

**Response:**
```json
{
    "userId": "userId",
    "username": "username",
    "email": "your@email.com",
    "token": "eynZW1lbnRDbGllbnQifQ.-dLCSNLIwQSl98iDTMwwSmUaVI9Thqly-UaORnF729A",
    "expiresAt": "0001-01-01T00:00:00",
    "role": "User"
}
```

### ✅ Get All Tasks
```http
GET /api/usertasks
```

### ✅ Get Task by ID
```http
GET /api/UserTasks/byId/{id}
```

### ✅ Create a New Task
```http
POST /api/usertasks
Content-Type: application/json

{
    "title": "New Task",
    "description": "Task details",
    "priority": "High",
    "dueDate": "2024-03-25"
}
```

### ✅ Update a Task
```http
PATCH /api/usertasks/{id}
Content-Type: application/json

{
    "title": "Updated Task",
    "description": "Updated details",
    "priority": "Medium",
    "dueDate": "2024-04-10"
}
```

### ✅ Delete a Task
```http
DELETE /api/usertasks/{id}
```

### ✅ Get Tasks by Priority
```http
GET /api/usertasks/priority/{priority}
```

## Project Structure
```
📂 TaskManagementAPI
 ┣ 📂 Controllers      # API Controllers
 ┣ 📂 Data             # Database Context 
 ┣ 📂 Middleware       # Global Error Handler 
 ┣ 📂 Migrations       # Database Migration 
 ┣ 📂 Models           # Entities & DTOs
 ┣ 📂 Repositories     # Data Access Layer
 ┣ 📂 Services         # Business Logic
 ┣ appsettings.json    # App Config
 ┗ Program.cs         # API Entry Point
```

## Contributing
1. Fork the repository
2. Create a feature branch (`feature-new-task-feature`)
3. Commit changes & push (`git commit -m "Added new task feature"`)
4. Open a pull request 🚀

## License
This project is **MIT Licensed**.

---

