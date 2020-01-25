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
    public class EventsController : Controller
    {
        private ApplicationUserManager _userManager;

        public EventsController()
        {
        }

        public EventsController(ApplicationUserManager userManager)
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

        // GET: Events
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Event> events = new List<Event>();

            var user = UserManager.FindById(User.Identity.GetUserId());
            var restaurantUser = UserManager.IsInRole(user.Id, "Restaurant");

            var restaurant = db.Restaurants.Where(r => r.OwnerId == user.Id).First();

            if (restaurant == null)
            {
                return HttpNotFound();
            }

            if (restaurantUser)
            {
                events = db.Events.Include(e => e.RestaurantId).Where(e => e.RestaurantId.RestaurantId == restaurant.RestaurantId).ToList();
                return View(events);
            }

            return View(db.Events.Include(e => e.RestaurantId).ToList());
        }

        // GET: Events/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        //GET: Events/createEventInRestaurant/id
        [Authorize(Roles = "Admin,Restaurant")]
        public ActionResult createEventInRestaurant(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var restaurant = db.Restaurants.Find(id);

            if (restaurant == null)
            {
                return HttpNotFound();
            }

            EventInRestaurant eventInRestaurant = new EventInRestaurant();
            eventInRestaurant.restaurantId = restaurant.RestaurantId;

            return View(eventInRestaurant);
        }

        // POST: Events/createEventInRestaurant/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Restaurant")]
        public ActionResult createEventInRestaurant(EventInRestaurant tableRest)
        {
            if (ModelState.IsValid)
            {
                var restaurant = db.Restaurants.Find(tableRest.restaurantId);
                Event e = tableRest.evnt;
                e.RestaurantId = restaurant;
                db.Events.Add(e);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }





        // GET: Events/Create
        [Authorize(Roles = "Admin,Restaurant")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Restaurant")]
        public ActionResult Create([Bind(Include = "EventId,Name,Date,Description,ImageUrl")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "Admin,Restaurant")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Restaurant")]
        public ActionResult Edit([Bind(Include = "EventId,Name,Date,Description,ImageUrl")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "Admin,Restaurant")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [Authorize(Roles = "Admin,Restaurant")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
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
