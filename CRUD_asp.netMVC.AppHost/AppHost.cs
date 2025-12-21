var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CRUD_asp_netMVC>("crud-asp-netmvc");

builder.Build().Run();
