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
    public partial class GetAPI
    {
        public static string GetSkillTypeData(string SkillType)
        {
            string data;
            try
            {
                using (var entity = new HirePressEntity())
                {
                    data = entity.MasterSkills.Where(x => x.SkillType == SkillType).Select(x=>x.SkillTypeData).SingleOrDefault();
                    if (data != null)
                        return data;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return "Not Found";
        }
        public static List<MasterJob> GetAllJob()
        {
            try
            {
                using (var entity = new HirePressEntity())
                {
                    var data = entity.MasterJobs.ToList();
                    if (data != null)
                        return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new List<MasterJob>();
        }
        public static MasterJob GetJob(string jobid)
        {
            try
            {
                using (var entity = new HirePressEntity())
                {
                    var data = entity.MasterJobs.Where(x => x.JobID == jobid).FirstOrDefault();
                    if (data != null)
                        return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new MasterJob();
        }
    }
}

