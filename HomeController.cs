using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Web.Models;
using AppContext = DataAccess.AppContext;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppContext db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            
            db = new AppContext(configuration["ConnectionString"]);
        }

        public async Task<IActionResult> Index()
        {

            var products = await db.Products.ToListAsync();
            
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration([FromForm] User user)
        {

            db.Users.AddRange(new List<User>
            {
                new User{FirstName = user.FirstName, LastName = user.LastName, Login = user.Login, Password = user.Password, PasswordConfirm = user.PasswordConfirm, Email = user.Email }
            });
            if (user.Password == user.PasswordConfirm)
            {
                db.SaveChanges();
                
                var newUser = db.Users.ToList();
                return RedirectToAction("Index", newUser);
            }
            else
            {
                return View();
            }            
            
        }
    }
}
