using HirePressCore.DataAccess;
using HirePressCore.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HirePressCore.Partial
{
    public partial class SetAPI
    {
        public static bool SetMasterSkills(string SkillType, string SkillTypeData)
        {
            bool flag;
            try
            {
                using (var entity = new HirePressEntity())
                {
                    var exist = entity.MasterSkills.Where(x => x.SkillType == SkillType).FirstOrDefault();
                    if (exist == null)
                    {
                        MasterSkills MS = new MasterSkills()
                        {
                            SkillType = SkillType,
                            SkillTypeData = SkillTypeData
                        };
                        entity.MasterSkills.Add(MS);
                        entity.SaveChanges();
                        flag = true;
                    }
                    else
                    {
                        exist.SkillTypeData = SkillTypeData;
                        entity.Entry(exist).State = System.Data.Entity.EntityState.Modified;
                        entity.SaveChanges();
                        flag = true;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return flag;
        }
        public static bool SetMasterJob(MasterJobModel model)
        {
            bool flag;
            try
            {
                using (var entity = new HirePressEntity())
                {
                    MasterJob MJ = new MasterJob()
                    {
                        JobID = Guid.NewGuid().ToString("N"),
                        ApplicationEmail = model.ApplicationEmail,
                        Category = model.Category,
                        ClosingDate = model.ClosingDate,
                        Company = model.Company,
                        Description = model.Description,
                        Education = model.Education,
                        JobTags = model.JobTags,
                        JobTitle = model.JobTitle,
                        Location = model.Location,
                        Tagline = model.Tagline,
                        Website = model.Website,
                        CreatedBy = "Admin",
                        CreatedDate = DateTime.Now,
                        IsFake = true,
                        IsClosed = false
                    };
                    var joburl = "/job-"+(model.Category + "-" + model.Company + "-" + model.JobTags + "-" + model.JobTitle
                               + "-" + model.Education + "-" + model.Location + "-" + model.Tagline).Replace("/","-") + "-" + MJ.JobID;
                    MJ.JobURL = joburl.Replace(",", "-").Replace("&", "-").Replace(".", "-").Replace(" ", "").ToLower();
                    entity.MasterJobs.Add(MJ);
                    entity.SaveChanges();
                    flag = true;
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }
    }
}

