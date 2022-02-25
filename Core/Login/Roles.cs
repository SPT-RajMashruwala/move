using KarKhanaBook.Model.Common;
using KarkhanaBookContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static KarKhanaBook.Model.Common.Result;

namespace KarKhanaBook.Core.Login
{
    public class Roles
    {

        public Result Add(Model.Login.Role roleModel)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                Role roledb = new Role();
                roledb.RoleName = roleModel.RoleName;

                context.Roles.InsertOnSubmit(roledb);
                context.SubmitChanges();
                var result = new Result()
                {
                    Message = "Role Added Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                       , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK

                };
                return result;
            }
        }
        public async Task<IEnumerable> View()
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.Roles
                             select new {obj.RoleID,obj.RoleName}
                             ).ToList();
                return dbobj;
            }

        }
        public async Task<IEnumerable> ViewById(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.Roles
                             where obj.RoleID == ID
                             select new { obj.RoleID,obj.RoleName}).ToList();
                return dbobj;
            }

        }
        public Result Update(Model.Login.Role roleModel, int roleID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.Roles
                             where obj.RoleID == roleID
                             select obj).SingleOrDefault();
                dbobj.RoleName = roleModel.RoleName;
                context.SubmitChanges();
                var result = new Result()
                {
                    Message = "Role Updated Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                   , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK

                };
                return result;

            }

        }
        public Result Delete(int roleID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.Roles
                             where obj.RoleID == roleID
                             select obj).SingleOrDefault();
                context.Roles.DeleteOnSubmit(dbobj);
                context.SubmitChanges();
                var result = new Result()
                {
                    Message = "Role Deleted Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                   , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK

                };
                return result;

            }

        }
    }
}
