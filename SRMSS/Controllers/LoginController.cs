
using Microsoft.AspNetCore.Mvc;
using SRMSS.Data;
using SRMSS.Models;

namespace SRMSS.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // LOGIN PAGE
        // =========================
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        // =========================
        // LOGIN
        // =========================
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            var user = _context.Users.FirstOrDefault(x =>
                x.Username == model.Username &&
                x.Password == model.Password);

            if (user == null)
            {
                ViewBag.Message = "Invalid Username or Password";
                return View();
            }

            if (user.Status != "Active")
            {
                ViewBag.Message = "Account is inactive";
                return View();
            }

            // Session
            HttpContext.Session.SetInt32("UserID", user.UserID);

            HttpContext.Session.SetString("Username",
                user.Username);

            HttpContext.Session.SetString("FullName",
                user.FullName);

            HttpContext.Session.SetString("ProfileImage", 
                user.ProfileImage ?? "");

            HttpContext.Session.SetString("Role",
                user.Role);

            return RedirectToAction("Index", "Dashboard");
        }

        // =========================
        // REGISTER PAGE
        // =========================
        public IActionResult Register()
        {
            return View();
        }

        // =========================
        // REGISTER
        // =========================
        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (_context.Users.Any(x => x.Username == model.Username))
            {
                ViewBag.Message = "Username already exists";
                return View();
            }

            if (_context.Users.Any(x => x.Email == model.Email))
            {
                ViewBag.Message = "Email already exists";
                return View();
            }

            User user = new User
            {
                FullName = model.FullName,
                Username = model.Username,
                Password = model.Password,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,

                // Default Role
                Role = "User",

                Status = "Active",

                CreatedDate = DateTime.Now
            };

            _context.Users.Add(user);

            _context.SaveChanges();

            TempData["Success"] =
                "Registration Successful";

            return RedirectToAction("Login");
        }

        // =========================
        // LOGOUT
        // =========================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}

