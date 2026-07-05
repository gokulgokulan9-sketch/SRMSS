using Microsoft.AspNetCore.Mvc;
using SRMSS.Data;
using SRMSS.Models;
using System.Linq;

namespace SRMSS.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var user = _context.Users.FirstOrDefault(x => x.UserID == userId);

            return View(user);
        }

        public IActionResult Edit()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
                return RedirectToAction("Login", "Login");

            var user = _context.Users.Find(userId);

            return View(user);
        }


        [HttpPost]
        public IActionResult Edit(User model, IFormFile? ProfilePhoto)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
                return RedirectToAction("Login", "Login");

            var user = _context.Users.Find(userId);

            if (user == null)
                return NotFound();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            if (ProfilePhoto != null)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/upload/profiles");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString() +
                                  Path.GetExtension(ProfilePhoto.FileName);

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ProfilePhoto.CopyTo(stream);
                }

                user.ProfileImage = fileName;
            }

            _context.SaveChanges();

            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetString("ProfileImage", user.ProfileImage ?? "");

            TempData["Success"] = "Profile updated successfully.";

            return RedirectToAction("Index");
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string CurrentPassword,
                                    string NewPassword,
                                    string ConfirmPassword)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
                return RedirectToAction("Login", "Login");

            var user = _context.Users.Find(userId);

            if (user == null)
                return NotFound();

            if (user.Password != CurrentPassword)
            {
                ViewBag.Error = "Current Password is incorrect.";
                return View();
            }

            if (NewPassword != ConfirmPassword)
            {
                ViewBag.Error = "New Password and Confirm Password do not match.";
                return View();
            }

            user.Password = NewPassword;

            _context.SaveChanges();

            ViewBag.Success = "Password changed successfully.";

            return View();
        }
    }
}