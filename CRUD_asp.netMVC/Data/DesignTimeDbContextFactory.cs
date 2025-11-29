//using CRUD_asp.netMVC.Data;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.Storage;
//using Microsoft.Extensions.DependencyInjection;
//using System;

//public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDBContext>
//{
//    public AppDBContext CreateDbContext(string[] args)
//    {
//        var builder = new DbContextOptionsBuilder<AppDBContext>();

//        // Set legacy timestamp behavior via AppContext before using Npgsql
//        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//        // Thay bằng connection string PostgreSQL/Neon của bạn (không commit mật khẩu thật vào repo)
//        var conn = "Host=ep-round-snow-ahlt7urr-pooler.c-3.us-east-1.aws.neon.tech; Database=neondb; Username=neondb_owner; Password=npg_9bInPigCj3yN; SSL Mode=VerifyFull; Channel Binding=Require;";
//        builder.UseNpgsql(conn);

//        return new AppDBContext(builder.Options);
//    }
//}
