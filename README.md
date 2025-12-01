# Sales E-commerce Platform
- An online shopping system designed to deliver a seamless buying experience for customers and efficient management tools for administrators.  
- Built with **ASP.NET Core MVC, C#, and SQL Server**, the platform provides a modern foundation for scalable e-commerce solutions. 

## Tech Stack
- **Backend**: ASP.NET Core 8 MVC, C#  
- **Database**: SQL Server, Entity Framework Core, Migrations
- **Frontend**: HTML, CSS, SCSS, Jquery, Bootstrap
- **SupportTech**: Git, GitHub, Ngrok, Postman
  
## Features
- **Product Management (CRUD)**  
  Admins can create, update, delete products, brands, and categories. Users interact with the data displayed.

- **User Authentication & Authorization**  
  Users and Admins can register, log in, and verify accounts via OTP sent to Gmail. Role-based access controls are enforced.

- **Password Recovery & Session Management**  
  Users/Admins can reset passwords using OTP via email and log out to switch accounts.

- **Product Display & Categorization**  
  Products are shown to Users, organized by brand and nested categories. Users can view details, add to cart, or purchase.

- **Contact via Homepage**  
  Users can send feedback or support requests to Admins through the contact form.

- **Search & Pagination**  
  Users can search products by name or description. Pagination is applied when product count exceeds page limits.

- **Shopping Cart Management**  
  Users can add, remove, and update product quantities in their cart.

- **Profile & Address Management**  
  Users can view and update their profile and manage shipping addresses during checkout.

- **QR Code Payment & Order Tracking**  
  Users can pay via QR code. Paid items are tracked and displayed for monitoring order status.

## Setup
1. Clone the repository: `git clone https://github.com/ThanhTuan208/Sales`
2. Install .NET SDK and SQL Server
3. Add appsettings.json:
 "ConnectionStrings": {
    "AppDBContext": "Data Source=your_server;Initial Catalog=name_db;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"
  },
"Smtp": {
    "Server": "smtp.gmail.com",
    "Port": "587",
    "User": "your_email",
    "Pass": " your_email_key_pass"
}
4. Run migrations: `dotnet ef database update` || `update-database`
5. Start the application: `dotnet run`

## Roadmap
- Improve cart performance
- Enhance responsive UI/UX
- Implement order cancellation and refund flow
- Add product reviews and ratings
