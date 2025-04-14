using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Account;
using CRUD_asp.netMVC.Models.Account.ActionViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using NuGet.Protocol.Plugins;

namespace CRUD_asp.netMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly RoleManager<Roles> _roleManager;
        private readonly AppDBContext _context;

        public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager, AppDBContext appDBContext, RoleManager<Roles> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = appDBContext;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register)
        {
            if (!ModelState.IsValid) return View(register);

            var user = new Users { UserName = register.UserName, Email = register.Email };
            var account = await _userManager.CreateAsync(user, register.Password);

            if (account.Succeeded)
            {
                string role = "Customer";

                if (register.Email.Contains("nhanvien", StringComparison.OrdinalIgnoreCase))
                {
                    role = "Staff";
                }
                else if (register.Email.Contains("nguyenthanhtuankrp1", StringComparison.OrdinalIgnoreCase))
                {
                    role = "Manager";
                }

                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new Roles { Name = role });
                }

                await _userManager.AddToRoleAsync(user, role);

                switch (role)
                {
                    case "Manager":
                        _context.Manager.Add(new Manager
                        {
                            UserName = user.UserName,
                            FullName = register.FullName,
                            Email = user.Email,
                            Password = register.Password
                        });
                        break;

                    case "Staff":
                        _context.Staff.Add(new Staff
                        {
                            UserName = user.UserName,
                            FullName = register.FullName,
                            Email = user.Email,
                            Password = register.Password
                        });
                        break;

                    case "Customer":
                        _context.Customer.Add(new Customer
                        {
                            UserName = user.UserName,
                            FullName = register.FullName,
                            Email = user.Email,
                            Password = register.Password
                        });
                        break;
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in account.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(register);
        }
    }
}
