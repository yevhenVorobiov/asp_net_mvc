using Hotel.Data;
using Hotel.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace NewHotel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomFeaturesController : Controller
    {
        private HotelDbContext db = new HotelDbContext();

        public ActionResult Index()
        {
            return View(db.RoomFeatures.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomFeature roomFeature = db.RoomFeatures.Find(id);
            if (roomFeature == null)
            {
                return HttpNotFound();
            }
            return View(roomFeature);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,AdditionalCost")] RoomFeature roomFeature)
        {
            if (ModelState.IsValid)
            {
                db.RoomFeatures.Add(roomFeature);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roomFeature);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomFeature roomFeature = db.RoomFeatures.Find(id);
            if (roomFeature == null)
            {
                return HttpNotFound();
            }
            return View(roomFeature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,AdditionalCost")] RoomFeature roomFeature)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomFeature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roomFeature);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomFeature roomFeature = db.RoomFeatures.Find(id);
            if (roomFeature == null)
            {
                return HttpNotFound();
            }
            return View(roomFeature);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomFeature roomFeature = db.RoomFeatures.Find(id);
            db.RoomFeatures.Remove(roomFeature);
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
