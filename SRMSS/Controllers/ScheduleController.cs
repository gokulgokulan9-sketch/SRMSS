
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRMSS.Data;
using SRMSS.Helpers;
using SRMSS.Models;
using SRMSS.Services;

namespace SRMSS.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

       

        private readonly EmailService _emailService;

        public ScheduleController(
            ApplicationDbContext context,
            EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // ==========================
        // INDEX
        // Admin + Staff + Supervisor
        // ==========================
        public async Task<IActionResult> Index(string searchString)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff",
                "Supervisor");

            if (access != null)
                return access;

            var schedules = from s in _context.Schedules
                            select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                schedules = schedules.Where(s =>
                    s.RouteCode.Contains(searchString) ||
                    s.BusNumber.Contains(searchString) ||
                    s.DriverName.Contains(searchString));
            }

            return View(await schedules.ToListAsync());
        }

        // ==========================
        // CREATE GET
        // Admin + Staff
        // ==========================
        public IActionResult Create()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            return View();
        }

        // ==========================
        // CREATE POST
        // Admin + Staff
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Schedule schedule)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            if (ModelState.IsValid)
            {
                _context.Add(schedule);

                var driver = _context.Drivers
    .FirstOrDefault(d => d.DriverName == schedule.DriverName);

                if (driver != null && !string.IsNullOrEmpty(driver.Email))
                {
                    string htmlBody = $@"
<html>
<body style='font-family:Arial;background:#f4f6f9;padding:20px;'>

<div style='max-width:700px;margin:auto;background:white;
border-radius:10px;overflow:hidden;
box-shadow:0 0 10px rgba(0,0,0,0.1);'>

<div style='background:#0d6efd;color:white;
padding:25px;text-align:center;'>

<h2>🚌 SRMSS</h2>
<p>Smart Route Management System</p>

</div>

<div style='padding:30px;'>

<h3>Hello {driver.DriverName},</h3>

<p>
A new bus schedule has been assigned to you.
Please review the details below.
</p>

<table style='width:100%;border-collapse:collapse;' border='1'>

<tr>
<td><b>Route Code</b></td>
<td>{schedule.RouteCode}</td>
</tr>

<tr>
<td><b>Bus Number</b></td>
<td>{schedule.BusNumber}</td>
</tr>

<tr>
<td><b>Schedule Date</b></td>
<td>{schedule.ScheduleDate:dd/MM/yyyy}</td>
</tr>

<tr>
<td><b>Departure Time</b></td>
<td>{schedule.DepartureTime}</td>
</tr>

<tr>
<td><b>Arrival Time</b></td>
<td>{schedule.ArrivalTime}</td>
</tr>

<tr>
<td><b>Status</b></td>
<td style='color:green;font-weight:bold;'>Assigned</td>
</tr>

</table>

<br/>

<p>
Please arrive on time and follow the assigned route schedule.
</p>

<br/>

Regards,<br/>
<b>SRMSS Administration Team</b>

</div>

<div style='background:#f8f9fa;
padding:15px;text-align:center;font-size:12px;'>

© 2026 SRMSS - Smart Route Management System

</div>

</div>

</body>
</html>";

                    await _emailService.SendEmailAsync(
                        driver.Email,
                        "🚌 New Bus Schedule Assigned",
                        htmlBody
                    );
                }

                _context.Activities.Add(new Activity
                {
                    Message = $"Schedule created for Bus {schedule.BusNumber} - Route {schedule.RouteCode}",
                    ActivityDate = DateTime.Now
                });

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(schedule);
        }

        // ==========================
        // EDIT GET
        // Admin + Staff
        // ==========================
        public async Task<IActionResult> Edit(int? id)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            if (id == null)
                return NotFound();

            var schedule = await _context.Schedules.FindAsync(id);

            if (schedule == null)
                return NotFound();

            return View(schedule);
        }

        // ==========================
        // EDIT POST
        // Admin + Staff
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Schedule schedule)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            if (id != schedule.ScheduleID)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(schedule);

                var driver = _context.Drivers
    .FirstOrDefault(d => d.DriverName == schedule.DriverName);

                if (driver != null && !string.IsNullOrEmpty(driver.Email))
                {
                    await _emailService.SendEmailAsync(
    driver.Email,
    "🚌 SRMSS - Schedule Updated",
    $@"
    <html>
    <body style='font-family:Segoe UI;background:#f4f6f9;padding:20px;'>

        <div style='max-width:700px;
                    margin:auto;
                    background:white;
                    border-radius:10px;
                    overflow:hidden;
                    box-shadow:0 2px 10px rgba(0,0,0,0.15);'>

            <div style='background:#0d6efd;
                        color:white;
                        padding:20px;
                        text-align:center;'>

                <h2>🚌 SRMSS</h2>
                <p>Smart Route Management System</p>

            </div>

            <div style='padding:25px;'>

                <h3>Hello {driver.DriverName},</h3>

                <p>
                    Your bus schedule has been updated.
                    Please review the details below.
                </p>

                <table style='width:100%;
                               border-collapse:collapse;'>

                    <tr>
                        <td style='padding:10px;border:1px solid #ddd;'>
                            Route Code
                        </td>

                        <td style='padding:10px;border:1px solid #ddd;'>
                            {schedule.RouteCode}
                        </td>
                    </tr>

                    <tr>
                        <td style='padding:10px;border:1px solid #ddd;'>
                            Bus Number
                        </td>

                        <td style='padding:10px;border:1px solid #ddd;'>
                            {schedule.BusNumber}
                        </td>
                    </tr>

                    <tr>
                        <td style='padding:10px;border:1px solid #ddd;'>
                            Schedule Date
                        </td>

                        <td style='padding:10px;border:1px solid #ddd;'>
                            {schedule.ScheduleDate:dd/MM/yyyy}
                        </td>
                    </tr>

                    <tr>
                        <td style='padding:10px;border:1px solid #ddd;'>
                            Departure Time
                        </td>

                        <td style='padding:10px;border:1px solid #ddd;'>
                            {schedule.DepartureTime}
                        </td>
                    </tr>

                    <tr>
                        <td style='padding:10px;border:1px solid #ddd;'>
                            Arrival Time
                        </td>

                        <td style='padding:10px;border:1px solid #ddd;'>
                            {schedule.ArrivalTime}
                        </td>
                    </tr>

                    <tr>
                        <td style='padding:10px;border:1px solid #ddd;'>
                            Status
                        </td>

                        <td style='padding:10px;border:1px solid #ddd;
                                   color:green;
                                   font-weight:bold;'>
                            {schedule.TripStatus}
                        </td>
                    </tr>

                </table>

                <br>

                <p>
                    Please report any schedule conflicts to the
                    transport administrator.
                </p>

                <p>
                    Regards,<br>
                    <strong>SRMSS Administration Team</strong>
                </p>

            </div>

            <div style='background:#f1f1f1;
                        text-align:center;
                        padding:15px;
                        font-size:12px;
                        color:#666;'>

                © 2026 SRMSS - Smart Route Management System<br>
                Automated Notification Email

            </div>

        </div>

    </body>
    </html>"
    );

                }
            
        

                _context.Activities.Add(new Activity
                {
                    Message = $"Schedule updated for Bus {schedule.BusNumber}",
                    ActivityDate = DateTime.Now
                });

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(schedule);
        }

        // ==========================
        // DELETE GET
        // Admin only
        // ==========================
        public async Task<IActionResult> Delete(int? id)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin");

            if (access != null)
                return access;

            if (id == null)
                return NotFound();

            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(m => m.ScheduleID == id);

            if (schedule == null)
                return NotFound();

            return View(schedule);
        }

        // ==========================
        // DELETE POST
        // Admin only
        // ==========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin");

            if (access != null)
                return access;

            var schedule = await _context.Schedules.FindAsync(id);

            if (schedule != null)
            {
                _context.Activities.Add(new Activity
                {
                    Message = $"Schedule deleted for Bus {schedule.BusNumber}",
                    ActivityDate = DateTime.Now
                });

                _context.Schedules.Remove(schedule);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

