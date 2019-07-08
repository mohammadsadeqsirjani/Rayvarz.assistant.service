using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rayvarz.invHServ.Library.Tools.Security
{
    public class TokenGenerator
    {

        public static string GetRandomToken
        {
            get
            {
                return MD5Builder.GenerateKey(Guid.NewGuid().ToString());
            }
        }
        public const string ROLE_TYPE_USER_PASSENGER = "00150002";
        public const string ROLE_TYPE_USER_DRIVER = "00150003";
        public const string ROLE_TYPE_USER_PORTALUSER = "00150001";
        public const string ROLE_TYPE_USER_DISPATCHER = "00150004";

        public const string STATUS_USER_PASSENGER_ACTIVE = "00140001";
        public const string STATUS_USER_PASSENGER_NOTACTIVE = "00140003";
        public const string STATUS_DRIVER_NORMAL = "00210001";
        public const string STATUS_DRIVER_SHIFTOFF = "00210003";
        public const string MSG_ERROR_DRIVER_USERISDEACTIVATED = "$$" + "وضعیت کاربری شما فعال نمی باشد. لطفا با مرکز پشتیبانی تماس حاصل فرمائید.";
        //public static User validateToken(string token, IRepository repo, string roleType = null)
        //{


        //    //#region کد موقتی تا درست شدن توکن
        //    //if (roleType == ROLE_TYPE_USER_PORTALUSER)
        //    //    return repo.DbCntx.Users.FirstOrDefault(); 
        //    //#endregion



        //    //try
        //    //{
        //    if (string.IsNullOrEmpty(token) || repo == null || repo.DbCntx == null)
        //        throw new Exception("Server Error: Invalid input parameters");
        //    Func<User, bool> e = u => u.token == MD5Builder.GenerateKey(token) && (string.IsNullOrEmpty(roleType) ? 1 == 1 : u.fk_RoleType == roleType);
        //    var user = repo.DbCntx.Users.FirstOrDefault(e);
        //    if (user == null)
        //        throw new Exception("$$" + "عدم دسترسی: شما با دستگاه دیگری وارد سیستم شده اید. درصورت تمایل،از حساب کاربری خود خارج شده و مجددا وارد برنامه شوید.");

        //    if (user.fk_RoleType == ROLE_TYPE_USER_PASSENGER)
        //    {
        //        if (user.fk_Status != STATUS_USER_PASSENGER_ACTIVE)
        //            throw new Exception(MSG_ERROR_DRIVER_USERISDEACTIVATED);
        //    }
        //    else if(user.fk_RoleType == ROLE_TYPE_USER_DRIVER || user.fk_RoleType == ROLE_TYPE_USER_DISPATCHER)
        //    {
        //        if(!new string[] { STATUS_DRIVER_NORMAL, STATUS_DRIVER_SHIFTOFF }.Contains(user.fk_Status))
        //            throw new Exception(MSG_ERROR_DRIVER_USERISDEACTIVATED);
        //    }

        //    return /*new resultModel<User>(*/user/*)*/;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return new resultModel<User>(ex);
        //    //}
        //}
    }
}
