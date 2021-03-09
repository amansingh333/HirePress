using HirePressCore.Model;
using HirePressCore.Partial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HirePress.Controllers
{
    public class apiController : Controller
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
        public JsonResult skills(string skilltype)
        {
            string data = GetAPI.GetSkillTypeData(skilltype);
            String[] skillsList = null;
            if(data != "Not Found")
            {
                skillsList = data.Split(new String[] { "," }, StringSplitOptions.None);
                return Json(skillsList, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult SetPostJobData(MasterJobModel model)
        {
            bool flag = SetAPI.SetMasterJob(model);
            return Json(flag);
        }
        [HttpGet]
        public JsonResult alljob()
        {
            var data = GetAPI.GetAllJob();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}