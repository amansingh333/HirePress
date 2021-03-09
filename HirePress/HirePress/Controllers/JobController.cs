using HirePressCore.Partial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HirePress.Controllers
{
    public class JobController : Controller
    {
        // GET: Job
        public ActionResult Index()
        {
            Uri uri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
            var alias = uri.PathAndQuery.Split(new String[] { "-" }, StringSplitOptions.None).Last();
            var data = GetAPI.GetJob(alias);
            return View(data);
        }
    }
}