using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRMSS.Data;
using SRMSS.Helpers;
using SRMSS.Models;

namespace SRMSS.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IActionResult History()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            var bookings = _context.Bookings
                .OrderByDescending(x => x.BookingDate)
                .ToList();

            ViewBag.Routes = _context.Routes.ToList();

            var busSummary = (from b in _context.Bookings
                              join r in _context.Routes
                              on b.RouteCode equals r.RouteCode
                              group new { b, r } by new
                              {
                                  b.BusNumber,
                                  b.RouteCode,
                                  r.RouteName
                              } into g
                              select new
                              {
                                  BusNumber = g.Key.BusNumber,
                                  RouteCode = g.Key.RouteCode,
                                  RouteName = g.Key.RouteName,
                                  TotalSeats = 40,
                                  BookedSeats = g.Count(),
                                  AvailableSeats = 40 - g.Count()
                              }).ToList();

            ViewBag.BusSummary = busSummary;

            return View(bookings);
        }
        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Bus List
        public IActionResult Index()
        {
            var routes = _context.Routes
                .Where(r => r.RouteStatus == "Active")
                .ToList();

            return View(routes);
        }

        // Seat Layout
        public IActionResult Seat(int id)
        {
            var route = _context.Routes.Find(id);

            if (route == null)
                return NotFound();

            ViewBag.Route = route;

            var bookedSeats = _context.Bookings
                .Where(x => x.RouteCode == route.RouteCode)
                .Select(x => x.SeatNumber)
                .ToList();

            ViewBag.BookedSeats = bookedSeats;

            return View();
        }

        // ==========================
        // BOOK GET
        // ==========================
        public IActionResult Book(int id, string seatNos)
        {
            var route = _context.Routes.Find(id);

            if (route == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(seatNos))
            {
                TempData["Error"] = "Please select at least one seat.";

                return RedirectToAction("Seat", new { id });
            }

            List<int> seats = seatNos
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            Booking booking = new Booking();

            booking.RouteCode = route.RouteCode;
            booking.BusNumber = route.AssignedBus;
            booking.TicketPrice = route.TicketPrice;
            booking.BookingDate = DateTime.Now;

            ViewBag.Route = route;
            ViewBag.SelectedSeats = seats;
            ViewBag.TotalAmount = seats.Count * route.TicketPrice;

            return View(booking);
        }
        public IActionResult BookingSuccess(int id)
        {
            var booking = _context.Bookings
                .FirstOrDefault(x => x.BookingID == id);

            if (booking == null)
                return NotFound();

            // Same passenger + same route + same journey date
            var bookings = _context.Bookings
                .Where(x =>
                    x.PassengerName == booking.PassengerName &&
                    x.RouteCode == booking.RouteCode &&
                    x.JourneyDate == booking.JourneyDate)
                .OrderBy(x => x.SeatNumber)
                .ToList();

            ViewBag.AllSeats = bookings
                .Select(x => x.SeatNumber)
                .ToList();

            ViewBag.TotalAmount = bookings
                .Sum(x => x.TicketPrice);

            var route = _context.Routes
    .FirstOrDefault(r => r.RouteCode == booking.RouteCode);

            ViewBag.RouteName = route?.RouteName;

            return View(booking);
        }

        // ==========================
        // BOOK POST
        // ==========================
        [HttpPost]
        public IActionResult Book(Booking booking, string seatNos)
        {
            if (string.IsNullOrWhiteSpace(seatNos))
            {
                TempData["Error"] = "Please select at least one seat.";

                return RedirectToAction("Seat",
                    new
                    {
                        id = _context.Routes
                        .First(x => x.RouteCode == booking.RouteCode)
                        .RouteID
                    });
            }

            List<int> seats = seatNos
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            foreach (var seat in seats)
            {
                Booking b = new Booking();

                b.RouteCode = booking.RouteCode;
                b.BusNumber = booking.BusNumber;
                b.SeatNumber = seat;

                b.PassengerName = booking.PassengerName;
                b.PassengerPhone = booking.PassengerPhone;
                b.PassengerEmail = booking.PassengerEmail;

                b.JourneyDate = booking.JourneyDate;

                b.TicketPrice = booking.TicketPrice;

                b.BookingDate = DateTime.Now;

                b.BookingStatus = "Booked";

                _context.Bookings.Add(b);
            }

            _context.SaveChanges();
            TempData["Success"] = "Booking completed successfully!";
            int bookingId = _context.Bookings
    .OrderByDescending(x => x.BookingID)
    .Select(x => x.BookingID)
    .First();



            return RedirectToAction("BookingSuccess",
    new
    {
        id = bookingId
    });
        }
    }
}