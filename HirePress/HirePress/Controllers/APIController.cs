using HirePressCore.Partial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HirePress.Controllers
{
    public class APIController : Controller
    {
        // GET: API

        public ActionResult TestAPI()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SetSkillsTypeData(string SkillType, string SkillTypeData)
        {
            bool flag = SetAPI.SetMasterSkills(SkillType, SkillTypeData);
            return Json(flag);
        }

        [HttpGet]
        public JsonResult GetSkillsTypeData(string SkillType)
        {
            string data = GetAPI.GetSkillTypeData(SkillType);
            String[] skillsList = null;
            if(data != "Not Found")
            {
                skillsList = data.Split(new String[] { "," }, StringSplitOptions.None);
                return Json(skillsList, JsonRequestBehavior.AllowGet);
            }
            return Json(false);
        }
    }
}