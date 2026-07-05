
using Microsoft.AspNetCore.Mvc;
using SRMSS.Data;
using SRMSS.Helpers;
using SRMSS.Models;

namespace SRMSS.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // INDEX
        // ==========================
        public IActionResult Index(string search)
        {
            var access = RoleHelper.CheckRole(HttpContext, "Admin");

            if (access != null)
                return access;

            var users = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(x =>
                    x.FullName.Contains(search) ||
                    x.Username.Contains(search) ||
                    x.Role.Contains(search));
            }

            return View(users.ToList());
        }

        // ==========================
        // CREATE
        // ==========================
        public IActionResult Create()
        {
            var access = RoleHelper.CheckRole(HttpContext, "Admin");

            if (access != null)
                return access;

            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            var access = RoleHelper.CheckRole(HttpContext, "Admin");

            if (access != null)
                return access;

            if (_context.Users.Any(x => x.Username == user.Username))
            {
                ViewBag.Error = "Username already exists!";
                return View(user);
            }

            if (_context.Users.Any(x => x.Email == user.Email))
            {
                ViewBag.Error = "Email already exists!";
                return View(user);
            }

            user.CreatedDate = DateTime.Now;

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["Success"] = "User created successfully.";

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // EDIT
        // ==========================
        public IActionResult Edit(int id)
        {
            var access = RoleHelper.CheckRole(HttpContext, "Admin");

            if (access != null)
                return access;

            var user = _context.Users.Find(id);

            if (user == null)
                return NotFound();

            return View(user);
        }
        

        [HttpPost]
        public IActionResult Edit(User user)
        {
            var access = RoleHelper.CheckRole(HttpContext, "Admin");

            if (access != null)
                return access;

            bool usernameExists = _context.Users.Any(x =>
                x.Username == user.Username &&
                x.UserID != user.UserID);

            if (usernameExists)
            {
                ViewBag.Error = "Username already exists!";
                return View(user);
            }

            bool emailExists = _context.Users.Any(x =>
                x.Email == user.Email &&
                x.UserID != user.UserID);

            if (emailExists)
            {
                ViewBag.Error = "Email already exists!";
                return View(user);
            }

            _context.Users.Update(user);
            _context.SaveChanges();

            TempData["Success"] = "User updated successfully.";

            return RedirectToAction(nameof(Index));
        }



        // ==========================
        // DELETE
        // ==========================
        public IActionResult Delete(int id)
        {
            var access = RoleHelper.CheckRole(HttpContext, "Admin");

            if (access != null)
                return access;

            var user = _context.Users.Find(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var access = RoleHelper.CheckRole(HttpContext, "Admin");

            if (access != null)
                return access;

            var user = _context.Users.Find(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            TempData["Success"] = "User deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}

