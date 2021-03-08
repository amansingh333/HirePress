using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HirePress.Controllers
{
    [RoutePrefix("")]
    public class JobController : Controller
    {
        // GET: Job
        [Route("{Alias?}")]
        public ActionResult Index(string Alias)
        {
            ViewBag.Alias = Alias;
            return View();
        }
    }
}