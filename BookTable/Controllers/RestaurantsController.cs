using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookTable.Database;
using BookTable.Models.DatabaseModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;


namespace BookTable.Controllers
{
    public class RestaurantsController : Controller
    {
        private ApplicationUserManager _userManager;

        public RestaurantsController()
        {
        }

        public RestaurantsController(ApplicationUserManager userManager)
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

        // GET: Restaurants
        [Authorize(Roles = "User,Admin,Restaurant")]
        public ActionResult Index()
        {
            List<Restaurant> restaurant = new List<Restaurant>();

            var user = UserManager.FindById(User.Identity.GetUserId());
            var restaurantUser = UserManager.IsInRole(user.Id, "Restaurant");

            if(restaurantUser)
            {
                return View(db.Restaurants.Where(rest => rest.OwnerId == user.Id).ToList());
            }
           
            return View(db.Restaurants.ToList());
        }

        // GET: Restaurants/Details/5
        [Authorize(Roles = "User,Admin,Restaurant")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // GET: Restaurants/Create
        [Authorize(Roles = "Admin,Restaurant")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Restaurant")]
        public ActionResult Create([Bind(Include = "RestaurantId,Name,Category,Description")] Restaurant restaurant)
        {
            

            var user = UserManager.FindById(User.Identity.GetUserId());
            string ownID = user.Id;

            if (ModelState.IsValid && ownID.Length > 0)
            {
                restaurant.OwnerId = ownID;
                db.Restaurants.Add(restaurant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        [Authorize(Roles = "Admin,Restaurant")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Restaurant")]
        public ActionResult Edit([Bind(Include = "RestaurantId,Approved,Name,Category,Description,Rating")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(restaurant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        [Authorize(Roles = "Admin,Restaurant")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [Authorize(Roles = "Admin,Restaurant")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);


            List<Table> tablesRest = db.Tables.Include(t => t.Restaurant).Where(t => t.Restaurant.RestaurantId == id).ToList();

            foreach (Table tab in tablesRest )
            {
                db.Tables.Remove(db.Tables.Find(tab.TableId));
            }

            db.Restaurants.Remove(restaurant);
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
