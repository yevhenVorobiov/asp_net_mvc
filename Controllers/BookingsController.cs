using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNet.Identity;

namespace NewHotel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookingsController : Controller
    {
        private HotelDbContext db = new HotelDbContext();

        public ActionResult Index()
        {
            return View(db.Bookings.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Include(b => b.Visitor)
                .Include(b => b.Room)
                .FirstOrDefault(b => b.Id == id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create([Bind(Include = "Id,StartDate,EndDate,VisitorsCount,Comment")] Booking booking,
            [Bind(Include = "Id,FirstName,LastName,PhoneNumber,Email")]Visitor visitor, int roomId)
        {
            Room room = GetRoom(roomId);
            if (room == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Someting went wrong!:( Please, retry");
            }
            if (room.MaxVisitorsCount < booking.VisitorsCount)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Too much visitors for this room. Maximum number is " + room.MaxVisitorsCount);
            }
            Visitor dbVisitor = GetVisitor(visitor);
            if (dbVisitor == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Some of your personal data is missing on in worng format. Please, retry");
            }
            booking.State = BookingState.PENDING;
            booking.Visitor = dbVisitor;
            booking.Room = room;
            booking.FinalPrice = (double)Session["finalPrice"];
            ModelState.Clear();
            if (!TryValidateModel(booking))
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Some of impopratant data is missing on in worng format. Please, retry");
            }
            dbVisitor.Bookings.Add(db.Bookings.Add(booking));
            db.SaveChanges();
            return Json(new { message = "Your have successfully booked a room!" });
        }

        private Visitor GetVisitor(Visitor visitor)
        {
            if (!ValidateVisitor(visitor))
            {
                return null;
            }
            var dbVisitor = db.Visitors.Where(v => v.Email.Equals(visitor.Email, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
            if (dbVisitor == null)
            {
                dbVisitor = db.Visitors.Add(visitor);
                db.SaveChanges();
            }
            if (dbVisitor.Bookings == null)
            {
                dbVisitor.Bookings = new List<Booking>();
            }
            return dbVisitor;
        }

        private Room GetRoom(int roomId)
        {
            return db.Rooms.FirstOrDefault(r => r.Id == roomId);
        }

        private bool ValidateVisitor(Visitor visitor)
        {
            var context = new ValidationContext(visitor, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            return Validator.TryValidateObject(visitor, context, results);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BookingState state)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return RedirectToAction("Index");
            }
            booking.State = state;
            booking.Employee = db.Users.Find(User.Identity.GetUserId());
            db.Entry(booking).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
