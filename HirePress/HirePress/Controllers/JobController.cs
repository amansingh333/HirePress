using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HirePress.Controllers
{
    //[RoutePrefix("job")]
    public class JobController : Controller
    {
        // GET: Job
        //[Route("{Alias?}")]
        public ActionResult Index()
        {
            Uri uri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
            ViewBag.Alias = uri.PathAndQuery;
            return View();
        }
    }
}