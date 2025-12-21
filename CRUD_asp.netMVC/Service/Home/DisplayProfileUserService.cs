
using CRUD_asp.netMVC.Common;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Generic;
using StackExchange.Redis;
using System.Security.Cryptography.Xml;

namespace CRUD_asp.netMVC.Service.Home
{
    public class DisplayProfileUserService : IDisplayProfileUserService
    {
        private readonly AppDBContext _dbContext;

        public DisplayProfileUserService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> HandleProfileDisplayAsync(string? options)
        {
            if (string.IsNullOrEmpty(options))
            {
                return "_MyProfilePartial";
            }

            return options.ToLower() switch
            {
                "profile" => "_MyProfilePartial",
                "surplus" => "_SurplusManagerPartial",
                _ => "_MyProfilePartial"
            };
        }
    }
}
