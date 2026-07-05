using Microsoft.AspNetCore.Mvc;
using SRMSS.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SRMSS.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var notifications = _context.Activities
       .OrderByDescending(a => a.ActivityDate)
       .Take(10)
       .ToList();

           
            return View(notifications);
        }

        [HttpPost]
       
        public IActionResult MarkAsRead(int id)
        {
            var activity = _context.Activities.Find(id);

            if (activity != null)
            {
                activity.IsRead = true;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ClearAll()
        {
            _context.Activities.RemoveRange(_context.Activities);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }

}