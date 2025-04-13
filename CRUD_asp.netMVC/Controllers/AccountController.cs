using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Account;
using CRUD_asp.netMVC.Models.Account.ActionViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace CRUD_asp.netMVC.Controllers
{
    public class AccountController : Controller
    {
        public readonly UserManager<Users> UserManager; 
        public readonly SignInManager<Users> SignInManager; 
        public readonly AppDBContext AppDBContext;

        public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager, AppDBContext appDBContext)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            AppDBContext = appDBContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> Register(Register Register)
        //{
        //    if(!ModelState.IsValid) return View(Register);

        //    var user = new Users { UserName = Register.UserName, Email = Register.Email };
        //    var account = await UserManager.CreateAsync(user, Register.Password);

        //    return await
        //}
    }
}
