using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace woontest.Controllers
{
    public class AdminAreaController : Controller
    {
        // GET: AdminArea
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}