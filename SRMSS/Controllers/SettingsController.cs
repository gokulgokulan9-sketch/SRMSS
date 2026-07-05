using Microsoft.AspNetCore.Mvc;
using SRMSS.Data;
using SRMSS.Models;

namespace SRMSS.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===========================
        // SETTINGS PAGE
        // ===========================
        public IActionResult Index()
        {
            var setting = _context.SystemSettings.FirstOrDefault();

            if (setting == null)
            {
                setting = new SystemSetting();
            }

            return View(setting);
        }

        [HttpPost]
        public IActionResult Save(SystemSetting model, IFormFile? LogoFile)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index");
            }
            var setting = _context.SystemSettings.FirstOrDefault();

            if (setting == null)
                return NotFound();

            setting.CompanyName = model.CompanyName;
            setting.PhoneNumber = model.PhoneNumber;
            setting.Email = model.Email;
            setting.Address = model.Address;
            setting.Website = model.Website;

            if (LogoFile != null && LogoFile.Length > 0)
            {
                string folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/upload/settings");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName = Guid.NewGuid().ToString()
                                + Path.GetExtension(LogoFile.FileName);

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    LogoFile.CopyTo(stream);
                }

                setting.Logo = fileName;
            }

            _context.SaveChanges();

            TempData["Success"] = "System Settings updated successfully.";

            return RedirectToAction("Index");
        }

    }
}