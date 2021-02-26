using HirePressCore.DataAccess;
using HirePressCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HirePressCore.Partial
{
    public partial class Util
    {
        public static List<Master_FlagModel> GetAllFlags()
        {
            List<Master_FlagModel> list = new List<Master_FlagModel>();
            try
            {
                using (var entity = new HirePressEntity())
                {
                    var data = entity.Master_Flag.ToList();
                    foreach (var a in data)
                    {
                        Master_FlagModel MFM = new Master_FlagModel()
                        {
                            FlagID = a.FlagID,
                            FlagName = a.FlagName,
                            IsFlag = a.IsFlag
                        };
                        list.Add(MFM);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public static string GetUserName(string email)
        {
            var username = "";
            try
            {
                using (var entity = new HirePressEntity())
                {
                    var data = entity.AspNetUsers.Where(x=>x.Email == email).FirstOrDefault();
                    username = data.FirstName + " " + data.LastName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return username;
        }

        public static void CreateUserDetails(RegisterViewModel rvm)
        {
            try
            {
                using (var entity = new HirePressEntity())
                {
                    var data = entity.AspNetUsers.Where(x => x.Email == rvm.Email).FirstOrDefault();
                    data.FirstName = rvm.FirstName;
                    data.LastName = rvm.LastName;
                    entity.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    entity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
