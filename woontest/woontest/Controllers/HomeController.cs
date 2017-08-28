using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using woontest.Models;

namespace woontest.Controllers
{
    public class HomeController : Controller
    {
        private store db = new store();
        public async Task<ActionResult> Index()
        {   
            return View(await db.Categories.ToListAsync());
        }

        public RedirectToRouteResult Redict(int id)
        {
            if (id == 6)
            return RedirectToAction("GreatIdea","Products");

            if (id == 5)
            return RedirectToAction("HappyBox","Products");

            return RedirectToAction("List", "Products", new { idCategory = id });
        }

        public ActionResult Reviews()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Delivery()
        {
            return View();
        }
    }
}