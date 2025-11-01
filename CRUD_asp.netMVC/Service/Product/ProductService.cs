using CRUD_asp.netMVC.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CRUD_asp.netMVC.Service.Product
{

    public class ProductService
    {
        private readonly AppDBContext _db;
        private readonly IDbConnection _dbConn;
        private readonly IDbContextFactory<AppDBContext> _factory;

        public ProductService(AppDBContext db, IDbConnection dbConn, IDbContextFactory<AppDBContext> factory)
        {
            _db = db;
            _dbConn = dbConn;
            _factory = factory;
        }
    }

}
