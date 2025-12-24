using CRUD_asp.netMVC.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace CRUD_asp.netMVC.Extensions.Products
{
    public static class ProductQueryExtensions
    {
        public static string? NameProductById(AppDBContext _dbContext, int id)
        {
            return _dbContext.Products.FirstOrDefault(p => p.ID == id)?.Name ?? "Không hiển thị!";
        }
    }
}
