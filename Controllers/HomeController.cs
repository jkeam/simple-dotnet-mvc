using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using SimpleDotnetMvc.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace SimpleDotnetMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            AppUser user = new AppUser(User);

            // Keycloak mapped fields
            //return View(new UserViewModel {
            //    Email =  getClaim(claims, ClaimTypes.Email),
            //    FirstName = getClaim(claims, ClaimTypes.GivenName),
            //    LastName = getClaim(claims, ClaimTypes.Surname)
            //});

            // Azure Ad
            return View(new UserViewModel {
                Id = user.GetId(),
                Email = user.GetEmail(),
                Name = user.GetName()
            }); ;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
