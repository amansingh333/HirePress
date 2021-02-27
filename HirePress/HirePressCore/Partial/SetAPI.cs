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
    }
}

