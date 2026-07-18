# Task Tracker API

This is a robust RESTful API for a Task Tracker application built with **.NET 8**, **Entity Framework Core**, and **PostgreSQL**. The project follows a clean **Layered Architecture** pattern with DTO mapping, global exception handling, pagination, sorting, and unit testing.

---

## 🛠 Prerequisites

Before running the application, ensure you have the following installed:
* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [PostgreSQL Database Server](https://www.postgresql.org/download/)
* [Postman](https://www.postman.com/downloads/) (for API testing) or use the built-in Swagger UI

---

## ⚙️ Configuration (App Settings Example)

Create or update the `appsettings.json` (or `appsettings.Development.json`) file in the root of the API project with your local PostgreSQL connection string:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=TaskTrackerDb;Username=postgres;Password=YOUR_POSTGRES_PASSWORD"
  }
}
