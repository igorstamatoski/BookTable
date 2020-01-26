using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookTable.Database;
using BookTable.Models.DatabaseModels;
using BookTable.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BookTable.Controllers
{
    public class ReservationsController : Controller
    {
        private ApplicationUserManager _userManager;

        public ReservationsController()
        {
        }

        public ReservationsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private BookTableContext db = new BookTableContext();

        // GET: Reservations
        [Authorize(Roles = "User,Admin,Restaurant")]
        public ActionResult Index()
        {
            List<Reservation> reservations = new List<Reservation>();

            var user = UserManager.FindById(User.Identity.GetUserId());
            var adminUser = UserManager.IsInRole(user.Id, "Admin");
            var restaurantUser = UserManager.IsInRole(user.Id, "Restaurant");
            var ordinaryUser = UserManager.IsInRole(user.Id, "User");

            if(adminUser)
            {
                var res = db.Reservations.Include(r => r.Event).Include(r => r.Table).Include(r => r.Table.Restaurant).ToList();

                foreach (Reservation r in res)
                {
                    string usr = UserManager.FindById(r.Idto).Email;
                    r.Idto = usr;
                }

                return View(res);

            } else if(restaurantUser)
            {
                var res = new List<Reservation>();
                if (db.Restaurants.Where(r => r.OwnerId == user.Id).ToList().Count() > 0)
                {

                    Restaurant rest = db.Restaurants.Where(r => r.OwnerId == user.Id).First();
                    res = db.Reservations.Include(r => r.Event).Include(r => r.Table).Include(r => r.Table.Restaurant).Where(r => r.Idto == user.Id && r.Event.RestaurantId.RestaurantId == rest.RestaurantId).ToList();
                }

                foreach(Reservation r in res)
                {
                    string usr = UserManager.FindById(r.Idto).Email;
                    r.Idto = usr;
                }
               
                return View(res);
            }
            else if(ordinaryUser)
            {
                var res = db.Reservations.Include(r => r.Event).Include(r => r.Table).Include(r => r.Table.Restaurant).Where(r => r.Idto == user.Id).ToList();

                foreach (Reservation r in res)
                {
                    string usr = UserManager.FindById(r.Idto).Email;
                    r.Idto = usr;
                }

                return View(res);
            }

            
            return RedirectToAction("Index","Home");
        }

        // GET: Reservations/Details/5
        [Authorize(Roles = "User,Admin,Restaurant")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/makeReservation/EventId
        [HttpPost]
        [Authorize(Roles = "User,Admin,Restaurant")]
        public ActionResult makeReservation(TablesToBook model, int id)
        {

            var table = db.Tables.Include(t => t.Restaurant).Where(t => t.TableId == id).First();

            if(table == null)
            {
                return HttpNotFound();
            }

            var evnt = db.Events.Find(model.eventId);

            if (evnt == null)
            {
                return HttpNotFound();
            }

            var user = UserManager.FindById(User.Identity.GetUserId());

            if(user == null)
            {
                return HttpNotFound();
            }


            Reservation newReservation = new Reservation();
            newReservation.CreatedAt = DateTime.UtcNow.ToLocalTime();
            newReservation.Event = evnt;
            newReservation.Idto = user.Id;
            newReservation.Table = table;

            db.Reservations.Add(newReservation);
            db.SaveChanges();

            return RedirectToAction("confirmedReservation");

        }

        [Authorize(Roles = "User,Admin,Restaurant")]
        public ActionResult confirmedReservation()
        {
            return View();
        }




        // GET: Reservations/Create
        [Authorize(Roles = "User,Admin,Restaurant")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationId,User,Time")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reservation);
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "User,Admin,Restaurant")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin,Restaurant")]
        public ActionResult Edit([Bind(Include = "ReservationId,User,Time")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "User,Admin,Restaurant")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin,Restaurant")]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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
