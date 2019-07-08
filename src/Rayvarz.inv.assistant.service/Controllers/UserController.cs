
using Rayvarz.inv.assistant.service.Models;
using Rayvarz.inv.assistant.service.Models.ray;
using Rayvarz.inv.assistant.service.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rayvarz.inv.assistant.service.Controllers
{
    /// <summary>
    /// سرویس هاس مرتبط با موجودیت کاربران
    /// </summary>
    [RoutePrefix("api/User")]
    public class UserController : AdvancedApiController
    {



        private string _userId { get; set; }
        private string UserId
        {
            get
            {//get from Request.Headers.Authorization.Scheme; , singleThone
                if (!string.IsNullOrEmpty(_userId))
                    return _userId;
                //using (var db = new Entities())
                //{
                var hd = Request.Headers.FirstOrDefault(h => h.Key.Equals("UserId")).Value;
                var inputToken = hd.FirstOrDefault();
                if (string.IsNullOrEmpty(inputToken))
                    throw new Exception("input token is null or empty");

                //    db.Configuration.ProxyCreationEnabled = false;

                //}

                _userId = inputToken;
                return _userId;
            }
        }

        /// <summary>
        /// سرویس اهراز هویت
        /// </summary>
        /// <param name="loginData"></param>
        /// <returns></returns>
        [Route("Login")]
        [Exception]
        [HttpPost]
        public async Task<IHttpActionResult> Login(loginBindingModel loginData)
        {

            if (!ModelState.IsValid)
            {
                return new BadRequestActionResult(ModelState.Values);
            }
            if ( new string[] { "official_web", "official" }.Contains(ConfigurationManager.AppSettings["api"].ToLower()))
            {
                //return Ok("123456");
                var sha256 = new SHA256Managed();
                var sha256Bytes = Encoding.UTF8.GetBytes(loginData.Password);//رمز ورود به سیستم
                                                                             //var accessToken = "";
                var cryString = sha256.ComputeHash(sha256Bytes);
                var sha256Str = cryString.Aggregate(string.Empty, (current, t) => current + t.ToString("x2"));
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["RayvarzApiAddress"] + "/");
                    var arrayList = new ArrayList
                {
                    loginData.UserId, // User Id
                    sha256Str, //Password
                };
                    var response = client.PostAsJsonAsync("RayvarzApi/Core/Sso/Authenticate", arrayList).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string resContent = await response.Content.ReadAsStringAsync();
                        resContent = resContent.Replace("\"", "");
                        //accessToken = resContent;
                        if (string.IsNullOrEmpty(resContent))
                            return Unauthorized();
                        return Ok(resContent);
                    }
                    else
                        return Unauthorized();
                    //return new NotFoundActionResult(response.ReasonPhrase);
                }
            }
            else
            {
                using (var db = new Entities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    if (db.UserIds.Any(u => u.UserId1 == loginData.UserId && u.UserLtnName == loginData.Password))
                        return Ok("123456");
                    return Unauthorized();
                }
            }

        }
    }
}
