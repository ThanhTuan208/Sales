using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using User = CRUD_asp.netMVC.Models.Auth.Users;

namespace CRUD_asp.netMVC.Extensions.Users
{
    public static class UserQueryExtensions
    {
        public static string GetNameByHttpAccessor(IHttpContextAccessor accessor)
        {
            return accessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value ?? "Không tìm thấy";
        }

        public static string GetNameByUserId(DbSet<User> user, int userId)
        {
            var getName = user.Where(p => p.Id == userId).FirstOrDefault();
            if (getName == null)
            {
                return "Không hiển thị";
            }

            return getName.FirstName + "\n" + getName.LastName;
        }
    }
}
