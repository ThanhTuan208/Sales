# Sales E-commerce Platform

An e-commerce platform that enables customers to shop conveniently and helps administrators efficiently manage products, orders, and users.

## Live Demo

- Website: https://youngteam.io.vn/

- Account user: user_demo@gmail.com
- Account admin: admin_demo@gmail.com
- Password user/admin: Nhonaovay@1

## Tech Stack

- **Backend**: ASP.NET Core MVC (C#)
- **Database**: SQL Server, Entity Framework Core, Migrations
- **Frontend**: HTML, CSS, SCSS, jQuery, Bootstrap
- **Realtime & Background jobs**: SignalR, Hangfire, Redis
- **Tooling**: Git, GitHub, Ngrok, Postman

## Main Features

- Product Management (CRUD)
- Authentication & Authorization (OTP Email, Role-based)
- Password Recovery & Session Management
- Product Catalog & Filtering
- Search & Pagination
- Shopping Cart
- Profile & Address Management
- QR Payment & Order Tracking
- Contact Support

## Project Structure

```
CRUD_asp.netMVC
CRUD_asp.netMVC.AppHost
CRUD_asp.netMVC.ServiceDefaults
```

## Getting Started

### Clone repository

```bash
git clone https://github.com/ThanhTuan208/Sales
cd Sales
```

### Prerequisites

- .NET SDK
- SQL Server

### Configure appsettings.json

```json
{
  "ConnectionStrings": {
    "AppDBContext": "Data Source=your_server;Initial Catalog=name_db;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"
  },
  "Smtp": {
    "Server": "smtp.gmail.com",
    "Port": "587",
    "User": "your_email",
    "Pass": "your_email_app_password"
  }
}
```

### Run migrations

```bash
update-database || dotnet ef database update
```

### Run application

```bash
dotnet run
```

## Roadmap

- Improve cart performance
- Enhance responsive UI/UX
- Order cancellation & refund flow
- Product reviews & ratings
