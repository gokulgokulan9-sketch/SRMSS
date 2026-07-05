
using Microsoft.AspNetCore.Mvc;
using SRMSS.Models;
using System.Diagnostics;

namespace SRMSS.Controllers
{
    public class HomeController : Controller
    {
        // HOME PAGE
        public IActionResult Index()
        {
            // User already logged in
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            // Not logged in
            return RedirectToAction("Login", "Login");
        }

        // PRIVACY PAGE
        public IActionResult Privacy()
        {
            return View();
        }

        // ACCESS DENIED PAGE
        public IActionResult AccessDenied()
        {
            return View();
        }

        // ERROR PAGE
        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = System.Diagnostics.Activity.Current?.Id ??
                            HttpContext.TraceIdentifier
            });
        }
    }
}

