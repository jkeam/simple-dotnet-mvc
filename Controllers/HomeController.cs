using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            List<Claim> claims = User.Claims.ToList();

            // Keycloak mapped fields
            //return View(new UserViewModel {
            //    Email =  getClaim(claims, ClaimTypes.Email),
            //    FirstName = getClaim(claims, ClaimTypes.GivenName),
            //    LastName = getClaim(claims, ClaimTypes.Surname)
            //});

            // Azure Ad
            return View(new UserViewModel {
                Email = GetClaim(claims, "preferred_username"),
                Name = GetClaim(claims, "name")
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static string GetClaim(List<Claim> claims, string key)
        {
            Claim claim = claims.Find(claim => claim.Type.Equals(key));
            return claim.Value;
        }
    }
}
