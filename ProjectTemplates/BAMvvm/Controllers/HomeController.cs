using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Brevitee.Data;
using System.Web.Mvc;
using Brevitee.DaoRef;

namespace BAMvvm.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Tests()
        {
            Db.TryEnsureSchema<TestTable>();
            return View();
        }
    }
}
