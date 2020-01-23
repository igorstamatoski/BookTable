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

namespace BookTable.Controllers
{
    public class TablesController : Controller
    {
        private BookTableContext db = new BookTableContext();

        // GET: Tables
        public ActionResult Index()
        {
            return View(db.Tables.ToList());
        }

        // GET: Tables/Details/5
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Tables/tablesForRestaurant/5
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
