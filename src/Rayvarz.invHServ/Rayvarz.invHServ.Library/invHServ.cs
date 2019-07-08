using Rayvarz.invHServ.Library.Models.common;
using Rayvarz.invHServ.Library.Tools.Security;
using Rayvarz.invHServ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rayvarz.invHServ.Library
{
    public class invHServ
    {
        public resultModel<string> login(inputModel<loginReqModel> im)
        {
            //return new resultModel<string>();
            //return new resultModel<string>() { message = lrm.value.userName + "**" + lrm.value.password, result = true };
            try
            {
                if (im == null || im.value == null
                    || string.IsNullOrEmpty(im.value.userName)
                    || string.IsNullOrEmpty(im.value.password))
                    throw new Exception("$$" + "لطفا نام کاربری و رمز عبور را وارد نمایید.");

                using (var db = new raydbDataContext())
                {
                }
            }
            catch (Exception ex)
            {
                return new resultModel<string>(ex);
            }
        }

        public resultModel checkService(inputModel im)
        {
            try
            {
                return new resultModel();
            }
            catch (Exception ex)
            {
                return new resultModel(ex);
            }
        }
    }
}

