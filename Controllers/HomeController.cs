using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleDotnetMvc.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
            return View(new UserViewModel {
                Email =  getClaim(claims, ClaimTypes.Email),
                FirstName = getClaim(claims, ClaimTypes.GivenName),
                LastName = getClaim(claims, ClaimTypes.Surname)
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string getClaim(List<Claim> claims, string key)
        {
            Claim claim = claims.Find(claim => claim.Type.Equals(key));
            return claim.Value;
        }
    }
}
