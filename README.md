# 🧱 User Details App

---
## ⚙️ Setup Instructions (with Docker)

### Prerequisites

- Git
- Docker & Docker Compose
- .NET 8 SDK (only required for local development)
- Node.js (v22+) & Angular CLI v20+ (only required for local development)

Each service runs in its own container:

### 🏗️ Architecture Overview

- `ui` → Angular app served via Nginx  
- `api` → ASP.NET Core Web API  
- `postgres` → PostgreSQL database  

### 🐳 Running the Application with Docker

From the project root:
- bash: `docker compose up --build`

## 🚀 Features

- User registration and login
- JWT‑based authentication
- Secure password hashing
- PostgreSQL database with EF Core migrations
- Automatic migrations on startup
- Angular UI served via Nginx
- Fully containerized with Docker Compose
- CORS enabled for local development

---

## ⚙️ Setup Instructions (Locally)

### Prerequisites
- Node.js (v22+)
- Angular CLI (`npm install -g @angular/cli`) (v20+)
- .NET SDK (v8.0+)
- PG Admin (PostgreSQL)
- Git

## ⚙️ Instructions to run project

### .NET Backend
- Go to https://dotnet.microsoft.com/download and download SDK (v8.0+) if not intalled
- Run command to install Entity Framework (EF) tools `dotnet tool install --global dotnet-ef`
- Configure connection string in appsettings.json to your database
- Run command to create database and migrations `dotnet ef database update`
- Run API using Http/Https port (https://localhost:5066/swagger/index.html) or use `dotnet run` in terminal

### Angular frontend

- Go to (https://nodejs.org) and download Latest version of node if not installed
- check node version using (`node -v`)
- install Angular CLI (`npm install -g @angular/cli`) if not installed (v20+)
- Navigate to root directory of project and install node modules (`npm install`)
- Install bootstrap from root directory (`npm install @ng-bootstrap/ng-bootstrap@19`)
- Adjust ApiBaseUrl in environment.ts (`https://localhost:5066/api`) file to match api url if necessary
- run project using (`ng serve`) then go to `http://localhost:4200` on your browser

## 🔍 Available Endpoints

AuthController
- /api/auth/register
- /api/auth/login
- /api/auth/refresh-token

UserDetailsController (Protected)
- /api/userdetails?email=

## 🔐 Authentication Flow

- Register → creates user
- Login → returns accessToken + refreshToken
- Angular stores tokens in localStorage
- Interceptor attaches Authorization: Bearer <token>
- If token expires → interceptor calls /refresh-token
- If refresh token is invalid → user is logged out

## Unit Testing

### Backend Tests
- Navigate to the test project directory
- Run the tests using command: `dotnet test`




