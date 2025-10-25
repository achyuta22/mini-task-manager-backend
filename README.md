# Mini Task Manager Backend

This is the backend of the Mini Task Manager application, built using **ASP.NET Core** and **Entity Framework Core** with **SQLite** as the database.

---

## Features

- User authentication (Register & Login) using JWT
- CRUD operations for projects
- CRUD operations for tasks within projects
- Task scheduling considering dependencies and due dates
- Role-based data access (only the owner can view/manage their projects)

---

## Tech Stack

- **Backend:** ASP.NET Core 7.0
- **ORM:** Entity Framework Core
- **Database:** SQLite
- **Authentication:** JWT (JSON Web Tokens)
- **API Testing:** Postman / Swagger UI

---

## Setup Instructions

 1. Clone the repository

```bash
git clone https://github.com/yourusername/mini-task-manager-backend.git
cd mini-task-manager-backend

### 2. Restore dependencies
dotnet restore

3. Update Database

This project uses Entity Framework Core migrations:
dotnet ef database update
```bash
Make sure you have dotnet-ef installed globally. You can install it with:
dotnet tool install --global dotnet-ef

4. Run the Backend
dotnet run
