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
using Microsoft.AspNet.Identity.Owin;

namespace BookTable.Controllers
{
    public class TablesController : Controller
    {
        private ApplicationUserManager _userManager;

        public TablesController()
        {
        }

        public TablesController(ApplicationUserManager userManager)
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

        // GET: Tables
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Tables.ToList());
        }


        // GET: Tables/Details/5
        [Authorize(Roles = "User,Admin,Restaurant")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Table table = db.Tables.Find(id);
            var restaurantId = db.Tables.AsNoTracking().Include(d => d.Restaurant).Where(t => t.TableId == id).First().Restaurant.RestaurantId;
            ViewBag.RestId = restaurantId;

            if (table == null)
            {
                return HttpNotFound();
            }

            return View(table);
        }


        // GET: Tables/Create
        [Authorize(Roles = "Admin, Restaurant")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Restaurant")]
        public ActionResult Create([Bind(Include = "TableId,Seats,Avaliable")] Table table)
        {
            if (ModelState.IsValid)
            {
                db.Tables.Add(table);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(table);
        }

        // GET: Tables/Edit/5
        [Authorize(Roles = "Admin, Restaurant")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            var restaurantId = db.Tables.AsNoTracking().Include(d => d.Restaurant).Where(t => t.TableId == id).First().Restaurant.RestaurantId;
            ViewBag.RestId = restaurantId;
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // GET: Tables/tablesToBook/EventId
        [Authorize(Roles = "Admin, Restaurant,User")]
        public ActionResult tablesToBook(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var e = db.Events.Include(r => r.RestaurantId).Where(evnt => evnt.EventId == id).First();

            if (e == null)
            {
                return HttpNotFound();
            }

            var rest = db.Restaurants.Find(e.RestaurantId.RestaurantId);

            if (rest == null)
            {
                return HttpNotFound();
            }

            var reservations = db.Reservations.Where(r => r.Event.EventId == e.EventId).ToList();
            var restTables = db.Tables.Where(t => t.Restaurant.RestaurantId == rest.RestaurantId).ToList();
            bool flag = false;
            foreach(Table t in restTables)
            {
                flag = false;

                foreach(Reservation r in reservations)
                {
                    if(t.TableId == r.Table.TableId)
                    {
                        flag = true;
                        break;
                    }
                }

                if(flag)
                {
                    t.Avaliable = false;
                } else
                {
                    t.Avaliable = true;
                }

            }

            TablesToBook model = new TablesToBook();

            model.tables = restTables;
            model.restaurant = rest;
            model.eventId = (int)id;

            return View(model);
        }

        // GET: Tables/tablesForRestaurant/5
        [Authorize(Roles = "Admin, Restaurant, User")]
        public ActionResult tablesForRestaurant(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tables = db.Tables.Where(tbl => tbl.Restaurant.RestaurantId == id ).ToList();
            ViewBag.RestId = id;

            if (tables == null)
            {
                return HttpNotFound();
            }

            return View(tables);
        }

        // GET: Tables/createTableForRestaurant/5
        [Authorize(Roles = "Admin, Restaurant")]
        public ActionResult createTableForRestaurant(int? id)
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

            TableInRestaurant tableInRestaurant = new TableInRestaurant();
            tableInRestaurant.restaurantId = (int)id;

            return View(tableInRestaurant);
        }

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Restaurant")]
        public ActionResult createTableForRestaurant(TableInRestaurant tableRest)
        {
           
            if (ModelState.IsValid)
            {
                var restaurant = db.Restaurants.Find(tableRest.restaurantId);
                var tableInRest = tableRest.table;
                tableInRest.Restaurant = restaurant;
                db.Tables.Add(tableInRest);
                db.SaveChanges();
                return RedirectToAction("tablesForRestaurant", new { id = restaurant.RestaurantId });
            }

            return View(tableRest);
        }


        // POST: Tables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Restaurant")]
        public ActionResult Edit([Bind(Include = "TableId,Seats,Avaliable,Restaurant")] Table table)
        {
            if (ModelState.IsValid)
            {
                var restaurantId = db.Tables.AsNoTracking().Include(d => d.Restaurant).Where(t => t.TableId == table.TableId).First().Restaurant.RestaurantId;
               

                db.Entry(table).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("tablesForRestaurant", new { id = restaurantId });
            }
            return View(table);
        }

        // GET: Tables/Delete/5
        [Authorize(Roles = "Admin, Restaurant")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            var restaurantId = db.Tables.AsNoTracking().Include(d => d.Restaurant).Where(t => t.TableId == id).First().Restaurant.RestaurantId;
            ViewBag.RestId = restaurantId;

            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Restaurant")]
        public ActionResult DeleteConfirmed(int id)
        {
            Table table = db.Tables.Find(id);
            var restaurantId = db.Tables.AsNoTracking().Include(d => d.Restaurant).Where(t => t.TableId == id).First().Restaurant.RestaurantId;
            db.Tables.Remove(table);
            db.SaveChanges();
            return RedirectToAction("tablesForRestaurant", new { id = restaurantId });
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
