using HirePressCore.DataAccess;
using HirePressCore.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HirePressCore.Partial
{
    public partial class Util
    {
        public static bool GetFlag(string FlagName)
        {
            bool flag;
            try
            {
                using (var entity = new HirePressEntity())
                {
                    flag = entity.MasterFlags.Where(x=>x.FlagName == FlagName).Select(x=>x.IsFlag).SingleOrDefault();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return flag;
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
        public static List<GetRegisterViewModel> GetUserDetails()
        {
            List<GetRegisterViewModel> rvml = new List<GetRegisterViewModel>();
            try
            {
                using (var entity = new HirePressEntity())
                {
                    var users = entity.AspNetUsers;
                    foreach(var user in users)
                    {
                        GetRegisterViewModel rvm = new GetRegisterViewModel();
                        ArrayList al = new ArrayList();
                        foreach (var role in user.AspNetRoles)
                        {
                            al.Add(role.Name);
                        }
                        rvm.Name = user.FirstName + " " + user.LastName;
                        rvm.Email = user.Email;
                        rvm.UserID = user.Id;
                        rvm.Roles = al;
                        rvml.Add(rvm);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rvml;
        }


    }
}
