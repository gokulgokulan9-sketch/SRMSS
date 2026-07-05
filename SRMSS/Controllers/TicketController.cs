using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using QRCoder;
using SRMSS.Data;
using System.IO;
using System.Linq;
using SRMSS.Services;


namespace SRMSS.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly EmailService _emailService;

        public TicketController(
    ApplicationDbContext context,
    EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        private byte[] GeneratePdfBytes(int id)
        {
            var booking = _context.Bookings
                .FirstOrDefault(x => x.BookingID == id);

            if (booking == null)
                return Array.Empty<byte>();

            var bookings = _context.Bookings
                .Where(x =>
                    x.PassengerName == booking.PassengerName &&
                    x.RouteCode == booking.RouteCode &&
                    x.JourneyDate == booking.JourneyDate)
                .OrderBy(x => x.SeatNumber)
                .ToList();

            string seats = string.Join(", ",
                bookings.Select(x => x.SeatNumber));

            decimal total = bookings.Sum(x => x.TicketPrice);

            var route = _context.Routes
                .FirstOrDefault(r => r.RouteCode == booking.RouteCode);

            string routeName = route != null ? route.RouteName : "-";

            using MemoryStream stream = new MemoryStream();

            PdfWriter writer = new PdfWriter(stream);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            string logoPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "logo.png");

            if (System.IO.File.Exists(logoPath))
            {
                ImageData imageData = ImageDataFactory.Create(logoPath);

                Image logo = new Image(imageData);

                logo.SetWidth(100);
                logo.SetHeight(100);
                logo.SetHorizontalAlignment(HorizontalAlignment.CENTER);

                document.Add(logo);
            }

            var font = PdfFontFactory.CreateFont(
                iText.IO.Font.Constants.StandardFonts.HELVETICA);

            var boldFont = PdfFontFactory.CreateFont(
                iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);

            Table header = new Table(1);
            header.SetWidth(UnitValue.CreatePercentValue(100));

            Cell cell = new Cell();

            cell.Add(new Paragraph("SRMSS BUS E-TICKET")
                .SetFont(boldFont)
                .SetFontSize(22)
                .SetFontColor(ColorConstants.WHITE));

            cell.SetBackgroundColor(ColorConstants.BLUE);
            cell.SetTextAlignment(TextAlignment.CENTER);
            cell.SetPadding(15);

            header.AddCell(cell);

            document.Add(header);
            document.Add(new Paragraph(" "));

            Table table = new Table(2);
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetBorder(new SolidBorder(ColorConstants.BLACK, 1));

            bool alternate = false;

            void AddRow(string title, string value)
            {
                Cell c1 = new Cell();
                Cell c2 = new Cell();

                if (alternate)
                {
                    c1.SetBackgroundColor(new DeviceRgb(245, 245, 245));
                    c2.SetBackgroundColor(new DeviceRgb(245, 245, 245));
                }

                c1.Add(new Paragraph(title).SetFont(boldFont));
                c2.Add(new Paragraph(value).SetFont(font));

                table.AddCell(c1);
                table.AddCell(c2);

                alternate = !alternate;
            }

            AddRow("Booking ID", booking.BookingID.ToString());
            AddRow("Passenger", booking.PassengerName);
            AddRow("Phone", booking.PassengerPhone);
            AddRow("Email", booking.PassengerEmail);
            AddRow("Route Code", booking.RouteCode);
            AddRow("Route", routeName);
            AddRow("Bus", booking.BusNumber);
            AddRow("Seats", seats);
            AddRow("Journey Date", booking.JourneyDate.ToString("dd/MM/yyyy"));
            AddRow("No. of Seats", bookings.Count.ToString());
            AddRow("Ticket Price", $"Rs. {booking.TicketPrice:0.00}");

            Cell totalTitle = new Cell()
    .SetBackgroundColor(new DeviceRgb(235, 235, 235));

            totalTitle.Add(
                new Paragraph("Total Amount")
                    .SetFont(boldFont));

            table.AddCell(totalTitle);

            table.AddCell(
                new Cell().Add(
                    new Paragraph($"Rs. {total:0.00}")
                        .SetFont(boldFont)
                        .SetFontColor(ColorConstants.RED)));

            Cell statusTitle = new Cell()
                .SetBackgroundColor(new DeviceRgb(235, 235, 235));

            statusTitle.Add(
                new Paragraph("Status")
                    .SetFont(boldFont));

            table.AddCell(statusTitle);

            table.AddCell(
                new Cell().Add(
                    new Paragraph("✔ BOOKED")
                        .SetFont(boldFont)
                        .SetFontColor(new DeviceRgb(0, 100, 0))));

            document.Add(table);
            document.Add(new Paragraph(" "));

            document.Add(
                new Paragraph("Generated on : " +
                DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"))
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph(" "));

            string qrText =
        $@"SRMSS BUS E-TICKET

Booking ID : {booking.BookingID}
Passenger : {booking.PassengerName}
Route : {route?.RouteName}
Route Code : {booking.RouteCode}
Bus : {booking.BusNumber}
Seats : {seats}
Journey Date : {booking.JourneyDate:dd/MM/yyyy}
Ticket Price : Rs. {booking.TicketPrice:0.00}
Total Amount : Rs. {total:0.00}
Status : BOOKED";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            QRCodeData qrCodeData =
                qrGenerator.CreateQrCode(
                    qrText,
                    QRCodeGenerator.ECCLevel.Q);

            PngByteQRCode qrCode =
                new PngByteQRCode(qrCodeData);

            byte[] qrBytes = qrCode.GetGraphic(20);

            ImageData qrImageData =
                ImageDataFactory.Create(qrBytes);

            Image qrImage = new Image(qrImageData);

            qrImage.SetWidth(120);
            qrImage.SetHeight(120);
            qrImage.SetHorizontalAlignment(HorizontalAlignment.RIGHT);

            document.Add(new Paragraph(" "));
            document.Add(qrImage);

            document.Add(
                new Paragraph("Scan to view ticket details")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(10));

            document.Add(new Paragraph(" "));

            document.Add(
                new Paragraph("Thank you for choosing SRMSS")
                .SetFont(boldFont)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.CENTER));

            document.Add(
                new Paragraph("© Smart Route Management & Scheduling System")
                .SetFontSize(9)
                .SetTextAlignment(TextAlignment.CENTER));

            document.Close();

            return stream.ToArray();
        }
        public IActionResult Download(int id)
        {
            var booking = _context.Bookings
                .FirstOrDefault(x => x.BookingID == id);

            if (booking == null)
                return NotFound();

            byte[] pdfBytes = GeneratePdfBytes(id);

            return File(
                pdfBytes,
                "application/pdf",
                $"SRMSS_Ticket_{booking.BookingID}.pdf");
        }

        public async Task<IActionResult> SendTicketEmail(int id)
        {
            var booking = _context.Bookings
                .FirstOrDefault(x => x.BookingID == id);

            if (booking == null)
                return NotFound();

            byte[] pdfBytes = GeneratePdfBytes(id);

            await _emailService.SendEmailWithAttachmentAsync(
                booking.PassengerEmail,
                "Your SRMSS E-Ticket",
                @"
<h2 style='color:#0d6efd;'>🚌 SRMSS Bus E-Ticket</h2>

<p>Dear Passenger,</p>

<p>Thank you for choosing <b>Smart Route Management & Scheduling System (SRMSS)</b>.</p>

<p>Your booking has been confirmed successfully.</p>

<p>
<b>Booking ID :</b> " + booking.BookingID + @"<br/>
<b>Passenger :</b> " + booking.PassengerName + @"<br/>
<b>Route :</b> " + booking.RouteCode + @"<br/>
<b>Journey Date :</b> " + booking.JourneyDate.ToString("dd/MM/yyyy") + @"
</p>

<p>
Your E-Ticket is attached as a PDF.
Please keep it with you during your journey.
</p>

<hr/>

<p style='font-size:14px;'>

Kind Regards,<br/><br/>

<b>SRMSS Team (Gokul,Abi,Jathu)</b><br/>
Smart Route Management & Scheduling System<br/>
Faculty of Computing<br/>
ESU Campus<br/>
Jaffna Sri Lanka

</p>

<hr/>

<p style='color:gray;font-size:12px'>
This is an automatically generated email.
Please do not reply to this email.
</p>

<p style='color:#0d6efd;font-size:12px'>
© 2025 Smart Route Management & Scheduling System (SRMSS).<br/>
All Rights Reserved.
</p>
",
                pdfBytes,
                $"SRMSS_Ticket_{booking.BookingID}.pdf");

            TempData["Success"] =
"✅ Your E-Ticket has been sent successfully to your email.";

            return RedirectToAction("Index", "Booking");
        }
    }
}