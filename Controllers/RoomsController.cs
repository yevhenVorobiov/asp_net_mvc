using Hotel.Data;
using Hotel.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace NewHotel.Controllers
{
    public class RoomsController : Controller
    {
        private HotelDbContext db = new HotelDbContext();

        public ActionResult Index(DateTime? StartDate, DateTime? EndDate)
        {
            var rooms = db.Rooms
                .Include(r => r.Type)
                .Include(r => r.Features)
                .ToList();
            if (User.IsInRole("Admin")) {
                return View(rooms);
            }
            if (!StartDate.HasValue || !EndDate.HasValue)
            {
                UpdateUrls(rooms);
                return View(new List<Room>());
            }
            if (DateTime.Compare(StartDate.Value.Date, EndDate.Value.Date) >= 0
                || DateTime.Compare(DateTime.Today, StartDate.Value.Date) > 0)
            {
                ModelState.AddModelError(string.Empty, "Wrong dates range was specified.");
                return View(new List<Room>());
            }
            return View(FilterRooms(rooms, StartDate.Value, EndDate.Value));
        }

        private List<Room> FilterRooms(List<Room> rooms, DateTime startDate, DateTime endDate)
        {
            var result = rooms.ToList();
            var bookings = from b in db.Bookings select b;
            var boockedRooms = bookings.Where(booking => booking.State != BookingState.DECLINED && booking.State != BookingState.CLOSED)
                .Where(booking => (DbFunctions.TruncateTime(booking.StartDate) <= startDate.Date && DbFunctions.TruncateTime(booking.EndDate) >= endDate.Date)
                || (DbFunctions.TruncateTime(booking.StartDate) >= startDate.Date && endDate.Date >= DbFunctions.TruncateTime(booking.StartDate))
                || (DbFunctions.TruncateTime(booking.EndDate) >= startDate.Date && endDate.Date >= DbFunctions.TruncateTime(booking.EndDate)))
                .Select(booking => booking.Room)
                .ToList();
            result.RemoveAll(room => boockedRooms.Contains(room));
            UpdateUrls(rooms.ToList());
            return result;
        }

        [HttpPost]
        public ActionResult CalculateCost(int Id, DateTime StartDate, DateTime EndDate)
        {
            var room = db.Rooms.Include(r => r.Type)
                .Include(r => r.Features)
                .FirstOrDefault(r => r.Id == Id);
            if (room == null || (DateTime.Compare(StartDate.Date, EndDate.Date) >= 0))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var costPerDay = room.Type.BasicRate + room.Features.Select(f => f.AdditionalCost)
                .Aggregate(0D, (acc, x) => acc + x);
            double result = (EndDate.Date - StartDate.Date).TotalDays * costPerDay;
            Session["finalPrice"] = result;
            return Content(result.ToString());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var roomModel = new RoomViewModel();
            roomModel.RoomTypes = db.RoomTypes.ToList();
            roomModel.RoomFeatures = db.RoomFeatures.ToList()
                .Select(x => new FeatureCheckbox { Name = x.Title, Id = x.Id })
                .ToList();
            return View(roomModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(RoomViewModel roomViewModel)
        {
            var room = roomViewModel.Room;
            if (room == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var RoomType = db.RoomTypes.Find(roomViewModel.SelectedTypeId);
            room.Type = RoomType;
            room.Features = roomViewModel.RoomFeatures
                .Where(f => f.Checked)
                .Select(f => db.RoomFeatures.Find(f.Id))
                .Where(f => f != null)
                .ToList();
            ModelState.Clear();
            if (TryValidateModel(room))
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roomViewModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            var roomModel = new RoomViewModel();
            roomModel.Room = room;
            roomModel.RoomTypes = db.RoomTypes.ToList();
            var features = room.Features ?? db.RoomFeatures.ToList();
            roomModel.RoomFeatures = features.Select(x => new FeatureCheckbox { Name = x.Title, Id = x.Id })
                .ToList();
            return View(roomModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(RoomViewModel roomViewModel)
        {
            var room = roomViewModel.Room;
            if (room == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var RoomType = db.RoomTypes.Find(roomViewModel.SelectedTypeId);
            room.Type = RoomType;
            room.Features = roomViewModel.RoomFeatures
                .Where(f => f.Checked)
                .Select(f => db.RoomFeatures.Find(f.Id))
                .Where(f => f != null)
                .ToList();
            ModelState.Clear();
            if (TryValidateModel(room))
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void UpdateUrls(List<Room> rooms)
        {
            rooms.ForEach(room => room.ImageUrl = room.ImageUrl.StartsWith("http")
            ? room.ImageUrl
            : string.Format("/Content/Images/{0}", room.ImageUrl));
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
