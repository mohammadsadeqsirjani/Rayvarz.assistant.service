using Newtonsoft.Json;
using Rayvarz.inv.assistant.service.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using System.Web.Script.Serialization;

namespace Rayvarz.inv.assistant.service.Controllers
{
    public abstract class AdvancedApiController : ApiController
    {
        public object clientIp
        {
            get
            {
                try
                {

                    if (Request.Properties.ContainsKey("MS_HttpContext"))
                    {
                        return ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                    }
                    else if (Request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
                    {
                        RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)Request.Properties[RemoteEndpointMessageProperty.Name];
                        return prop.Address;
                    }
                    else if (HttpContext.Current != null)
                    {
                        return HttpContext.Current.Request.UserHostAddress;
                    }
                    else
                    {
                        return DBNull.Value;
                    }
                }
                catch
                {
                    return DBNull.Value;
                }
            }
        }
        public string conStr
        {
            get
            {
                var eb = new EntityConnectionStringBuilder();
                eb.Metadata = @"res://*/";
                eb.Provider = "System.Data.SqlClient";
                //eb.ProviderConnectionString = @"Data Source=192.168.1.105;Initial Catalog=_ab;Persist Security Info=True;User ID=sa;Password=sa";
                eb.ConnectionString = ConfigurationManager.ConnectionStrings["Entities"].ConnectionString;
                return eb.ProviderConnectionString;
            }
        }
        #region common headers
        /// <summary>
        /// رشته هویتی
        /// </summary>
        public object authorization
        {
            get
            {
                return Request.Headers.Authorization.Scheme.getDbValue();
            }
        }
        /// <summary>
        /// شناسه کاربر
        /// با شناسه کاربر وارد شده ست می شود
        /// TODO:  باید از خروجی سرویس اهراز هویت ست شود و نه هدر درخواست
        /// </summary>
        public string userId
        {
            get
            {
                try
                {
                    var hd = Request.Headers.FirstOrDefault(h => h.Key.Equals("UserId")).Value;
                    return hd.FirstOrDefault();
                }
                catch
                {
                    return null; // DBNull.Value;
                }
            }
        }
        /// <summary>
        /// کد انبار
        /// </summary>
        public string storeNo
        {
            get
            {
                return Request.Headers.FirstOrDefault(h => h.Key.Equals("StoreNo")).Value.FirstOrDefault();
            }
        }
        /// <summary>
        /// شعبه
        /// </summary>
        public object branch
        {
            get
            {
                try
                {
                    return Request.Headers.FirstOrDefault(h => h.Key.Equals("branch")).Value.FirstOrDefault();
                }
                catch
                {
                    return DBNull.Value;
                }
            }
        }

        #endregion
        #region Rayvarz Web Inventory Api
        public async Task<ApiValidationResult> GenerateDocument(InventoryJournal inventoryJournal, InventoryJournalLogicDto logicDto)
        {
            //try
            //{
            //var accessToken = await new AutenticateManager().Autenticate();

            using (var client = new HttpClient())
            {
                //آدرس وب سایتی که ای پی آی روی آن در حال اجرا است
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["RayvarzApiAddress"] + "/");

                //آماده کردن مبدل
                var jsonFormatter = new JsonMediaTypeFormatter();
                jsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                jsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

                //مقدار دهی بر اساس ورودی های سرویس مربوطه
                var arrayList = new ArrayList()
                {
                    inventoryJournal,
                    logicDto
                };

                //مقداردهی دسترسی ها
                client.DefaultRequestHeaders.Add("access_token", authorization.ToString()/*accessToken*/);

                client.Timeout = new TimeSpan(0, 2, 0);

                //فراخوانی ای پی آی
                var response = client.PostAsync("RayvarzApi/Inventory/CommonInventory/GenerateDocument",
                    arrayList,
                    jsonFormatter).Result;



                //خواندن پاسخی که از ای پی آی برگشته است
                var content = response.Content.ReadAsAsync<object>();

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("دسترسی شما به سیستم منقضی شده است. لطفا مجددا وارد سیستم شوید.");
                }
                if (content.Result != null)
                {
                    //return (EntitySavedResult) content.Result;
                    var strResult = content.Result.ToString();

                    if (strResult.Contains("Authentication"))
                        throw new UnauthorizedAccessException("دسترسی شما به سیستم منقضی شده است. لطفا مجددا وارد سیستم شوید.");

                    var output = JsonConvert.DeserializeObject<ApiValidationResult>(strResult);
                    return output;
                }

                return null;
            }

            //}
            //catch(UnauthorizedAccessException ex)
            //{

            //}
            //catch (Exception ex)
            //{
            //    return ex;
            //}
        }
        public class ApiValidationResult
        {
            public string id { get; set; }
            public int documentNo { get; set; }
            public List<string> validationResults { get; set; }
        }
        public class InventoryJournalItem
        {
            public int FiscalYear { get; set; }
            public string WarehouseId { get; set; }
            public byte InventoryDocumentTypeId { get; set; }
            public int DocumentNo { get; set; }
            public short DocumentRow { get; set; }
            public string ItemDataId { get; set; }
            public string InventoryRackId { get; set; }
            public Nullable<int> CenterId { get; set; }
            public Nullable<long> InventoryOrderId { get; set; }
            public Nullable<int> ItemReceiptTypeId { get; set; }
            public Nullable<byte> RequestTypeId { get; set; }
            public Nullable<int> ItemConsumptionTypeId { get; set; }
            public string SupplierId { get; set; }
            public Nullable<int> Center3Id { get; set; }
            public Nullable<byte> InventoryModificationReasonId { get; set; }
            public string Serial { get; set; }
            public Nullable<decimal> Qty { get; set; }
            public string UnitId { get; set; }
            public Nullable<decimal> CountedQty { get; set; }
            public Nullable<decimal> Weight { get; set; }
            public string NeedDate { get; set; }
            public Nullable<decimal> Amount { get; set; }
            public string DestinationWarehouseId { get; set; }
            public string FinancialDocumentNo { get; set; }
            public Nullable<decimal> TollAmount { get; set; }
            public Nullable<decimal> TaxAmount { get; set; }
            public string Description { get; set; }
            public Nullable<int> ReferenceDocumentNo { get; set; }
            public Nullable<short> ReferenceRowNo { get; set; }
            public string ReferenceItemDataId { get; set; }
            public string ReferenceWarehouseId { get; set; }
            public string InvoiceNo { get; set; }
            public string InvoiceDate { get; set; }
            public Nullable<int> OtherRefrenceDocumentNo { get; set; }
            public Nullable<short> BuyRequestDocumentRow { get; set; }
            public Nullable<byte> ReturnFromBuyReason { get; set; }
            public Nullable<byte> ReturnToWarehouseReason { get; set; }
            public string FromPlaque { get; set; }
            public string ToPlaque { get; set; }
            public Nullable<decimal> BuyRequestRemaining { get; set; }
            public Nullable<int> WeighBridgePaper { get; set; }
            public Nullable<int> Center4Id { get; set; }
            public Nullable<int> Center5Id { get; set; }
        }
        public class InventoryJournal
        {
            public int DocumentNo { get; set; }
            public int FiscalYear { get; set; }
            public string WarehouseId { get; set; }
            public byte InventoryDocumentTypeId { get; set; }
            public Nullable<int> DemandedCenterId { get; set; }
            public string SupplierId { get; set; }
            public Nullable<byte> ReturnFromBuyReasonId { get; set; }
            public Nullable<byte> ReturnToInventoryReasonId { get; set; }
            public Nullable<long> InventoryOrderId { get; set; }
            public Nullable<int> Center3Id { get; set; }
            public Nullable<int> CenterId { get; set; }
            public Nullable<int> GetterOrSenderDocumentNo { get; set; }
            public string DestinationWarehouseId { get; set; }
            public Nullable<long> DestinationInventoryOrderId { get; set; }
            public Nullable<int> DestinationCenterId { get; set; }
            public Nullable<int> OrderRegisterNo { get; set; }
            public string DocumentingCredibilityNo { get; set; }
            public string CreateDate { get; set; }
            public Nullable<int> ReferenceFiscalYear { get; set; }
            public Nullable<int> ReferenceDocumentNo { get; set; }
            string MultiReference { get; set; }
            public Nullable<byte> ReferenceDocumentTypeId { get; set; }
            public string IssueItemDataId { get; set; }
            public Nullable<int> OrginalDocumentno { get; set; }
            public Nullable<int> TransferDocumentNo { get; set; }
            public List<InventoryJournalItem> InventoryJournalItems { get; set; }
        }
        public class InventoryJournalLogicDto
        {
            //raysysspc
            public string ComputerName { get; set; }//نام کامپیوتر

            //InvHdrData, InvDtlData
            public bool HasRefrence { get; set; }//مرجع دار بودن سند جاری
            public string RefDocdate { get; set; }//تاریخ  سند مرجع
            public bool MultiReference { get; set; }//چند مرجعی
            public bool IsSaleIssueDoc { get; set; }//آیا سند سایر سیستمها است
            public bool IsStandardDocNo { get; set; }

        }
        #endregion
        public publicFilterModel filterModel { get; set; }
    }
    #region database utilities
    public enum autenticationMode
    {
        authenticateAsUser = 0,
        authenticateAsAnonymous,
        NoAuthenticationRequired
    }

    public class Repo : IDisposable
    {
        public string spName_;
        public Repo(AdvancedApiController controller, string spName, string funcId, autenticationMode AuthMode = autenticationMode.NoAuthenticationRequired, bool initAsReader = false, bool checkAccess = false)
        {
            spName_ = spName;
            theAutenticationMode = AuthMode;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConStr"].ConnectionString);
            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;
            //cmd.Parameters.AddWithValue("@clientLanguage", controller.clientLanguage);
            cmd.Parameters.AddWithValue("@userId", controller.userId.getDbValue());//TODO: باید از سرویس اهراز هویت دریافت شود و نه هدر درخواست
            cmd.Parameters.AddWithValue("@branch", controller.branch);
            cmd.Parameters.AddWithValue("@storeNo", controller.storeNo.getDbValue());
            cmd.Parameters.AddWithValue("@clientIp", controller.clientIp);
            if (initAsReader)
            {
                controller.filterModel = controller.filterModel ?? new publicFilterModel() { key = "", _fromIndex = 0, _take = 1000 };
                cmd.Parameters.AddWithValue("@key", controller.filterModel.key);
                cmd.Parameters.AddWithValue("@fromIndex", controller.filterModel._fromIndex);
                cmd.Parameters.AddWithValue("@take", controller.filterModel._take);
                cmd.Parameters.AddWithValue("@orderBy", controller.filterModel.orderBy);
                cmd.Parameters.AddWithValue("@isDescOrder", controller.filterModel.isDescOrder);
            }
            else
            {

                par_rCode = cmd.Parameters.Add("@rCode", SqlDbType.TinyInt);
                par_rCode.Direction = ParameterDirection.Output;

                par_rMsg = cmd.Parameters.Add("@rMsg", SqlDbType.VarChar, -1);
                par_rMsg.Direction = ParameterDirection.Output;


            }
            con.Open();

            if (theAutenticationMode == autenticationMode.NoAuthenticationRequired)
                return;

            cmdA = con.CreateCommand();
            cmdA.CommandType = CommandType.StoredProcedure;

            //par_sessionId = cmdA.Parameters.Add("@sessionId", SqlDbType.BigInt);
            //par_sessionId.Direction = ParameterDirection.InputOutput;
            //par_sessionId.Value = controller.sessionId;
            //cmdA.Parameters.AddWithValue("@sessionId", controller.sessionId);
            // cmdA.Parameters.AddWithValue("@appVersion", controller.appVersion);
            // cmdA.Parameters.AddWithValue("@checkPermission", controller.checkPermission);
            //par_authorization = cmdA.Parameters.Add("@authorization", SqlDbType.Char, 128);
            //par_authorization.Direction = ParameterDirection.InputOutput;
            //par_authorization.Value = controller.authorization;
            cmdA.Parameters.AddWithValue("@authorization", controller.authorization);

            par_unauthorized = cmdA.Parameters.Add("@unauthorized", SqlDbType.Bit);
            par_unauthorized.Direction = ParameterDirection.Output;


            if (theAutenticationMode == autenticationMode.authenticateAsAnonymous)
            {
                cmdA.CommandText = "SP_authenticateReq";
                //cmdA.Parameters.AddWithValue("@mobile", controller.mobile.getDbValue());
                //cmdA.Parameters.AddWithValue("@otpCode", controller.otpCode.getDbValue());

                cmdA.ExecuteNonQuery();
            }
            else if (theAutenticationMode == autenticationMode.authenticateAsUser)
            {
                cmdA.CommandText = "SP_authenticateUser";
                cmdA.Parameters.AddWithValue("@funcId", funcId);
                cmdA.Parameters.AddWithValue("@checkAccess", checkAccess);
                //cmdA.Parameters.AddWithValue("@storeNo", controller.storeNo.getDbValue());
                //cmdA.Parameters.AddWithValue("@orderId", controller.orderId.getDbValue());

                //par_appId = cmdA.Parameters.Add("@appId", SqlDbType.TinyInt);
                //par_appId.Direction = ParameterDirection.InputOutput;
                //par_appId.Value = controller.appId;

                par_userId = cmdA.Parameters.Add("@userId", SqlDbType.VarChar, 50);
                par_userId.Direction = ParameterDirection.Output;

                par_accessDenied = cmdA.Parameters.Add("@accessDenied", SqlDbType.Bit);
                par_accessDenied.Direction = ParameterDirection.Output;

                par_osType = cmdA.Parameters.Add("@osType", SqlDbType.TinyInt);
                par_osType.Direction = ParameterDirection.Output;

                par_appVersion = cmdA.Parameters.Add("@appVersion", SqlDbType.Money);
                par_appVersion.Direction = ParameterDirection.Output;


                //par_termsAndConditions_LastAccepted = cmdA.Parameters.Add("@termsAndConditions_LastAccepted", SqlDbType.BigInt);
                //par_termsAndConditions_LastAccepted.Direction = ParameterDirection.Output;


                par_staffId = cmdA.Parameters.Add("@staffId", SqlDbType.SmallInt);
                par_staffId.Direction = ParameterDirection.Output;

                cmdA.ExecuteNonQuery();
                //if (!unauthorized)
                //{
                cmd.Parameters.AddWithValue("@userId", par_userId.Value);
                //cmd.Parameters.AddWithValue("@appId", appId);
                //}

            }
            else
                throw new Exception("invalid autenticationMode");


        }
        #region Ado Objs
        public SqlConnection con;
        public SqlCommand cmd;
        private SqlCommand cmdA;
        public SqlDataReader sdr;
        public SqlDataAdapter sda;
        public DataSet ds;
        #endregion
        #region common SQL params
        SqlParameter par_appId;
        public byte appId
        {
            get
            {
                return Convert.ToByte(par_appId.Value);
            }
        }
        SqlParameter par_storeId;
        public long storeId
        {
            get
            {
                return Convert.ToInt64(par_storeId.Value);
            }
        }

        SqlParameter par_userId;
        public long userId
        {
            get
            {
                return Convert.ToInt64(par_userId.Value);
            }
        }
        SqlParameter par_osType;
        public byte? osType
        {
            get
            {
                return par_osType.Value.dbNullCheckByte(); //Convert.ToByte(par_osType.Value);
            }
        }


        SqlParameter par_appVersion;
        public byte? appVersion
        {
            get
            {
                return par_appVersion.Value.dbNullCheckByte(); //Convert.ToByte(par_appVersion.Value);
            }
        }



        //SqlParameter par_termsAndConditions_LastAccepted;
        //public long termsAndConditions_LastAccepted
        //{
        //    get
        //    {
        //        return Convert.ToInt64(par_termsAndConditions_LastAccepted.Value);
        //    }
        //}

        SqlParameter par_staffId;
        public short staffId
        {
            get
            {
                return Convert.ToInt16(par_staffId.Value);
            }
        }
        //SqlParameter par_sessionId;
        //public long sessionId
        //{
        //    get
        //    {
        //        return Convert.ToInt64(par_sessionId.Value);
        //    }
        //}
        private autenticationMode theAutenticationMode;
        SqlParameter par_authorization;
        public string authorization
        {
            get
            {
                return Convert.ToString(par_authorization.Value);
            }
        }
        SqlParameter par_unauthorized;
        public bool unauthorized
        {
            get
            {
                if (theAutenticationMode == autenticationMode.NoAuthenticationRequired)
                    return false;
                return Convert.ToBoolean(par_unauthorized.Value);
            }
        }
        SqlParameter par_rCode;
        public byte rCode
        {
            get
            {
                return Convert.ToByte(par_rCode.Value);
            }
        }
        SqlParameter par_rMsg;
        public string rMsg
        {
            get
            {
                return Convert.ToString(par_rMsg.Value);
            }
        }
        public messageModel operationMessage
        {
            get
            {
                return new messageModel(rMsg);
            }
        }

        SqlParameter par_accessDenied;
        public bool accessDenied
        {
            get
            {
                return Convert.ToBoolean(par_accessDenied.Value);
            }
        }
        #endregion
        public void Dispose()
        {
            if (cmdA != null)
                cmdA.Dispose();
            if (sdr != null)
                sdr.Dispose();
            if (cmd != null)
                cmd.Dispose();
            if (ds != null)
                ds.Dispose();
            if (sda != null)
                sda.Dispose();
            if (con != null)
            {
                con.Close();
                con.Dispose();
            }
        }

        #region Ado Functions
        public void ExecuteNonQuery()
        {
            //if (unauthorized)
            //    throw new Exception("unauthorized!");
            cmd.ExecuteNonQuery();
        }
        public void ExecuteReader()
        {
            if (unauthorized)
                throw new Exception("unauthorized!");
            sdr = cmd.ExecuteReader();
        }
        public void ExecuteAdapter()
        {
            if (unauthorized)
                throw new Exception("unauthorized!");
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
        }
        #endregion
    }
    #endregion
    #region Advanced Action Results

    public class HttpActionResult : IHttpActionResult
    {
        private readonly string _message;
        private readonly HttpStatusCode _statusCode;

        public HttpActionResult(HttpStatusCode statusCode, string message)
        {
            _statusCode = statusCode;
            _message = string.Format("{{\"msg\" : \"{0}\"}}", message);
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_message)
            };
            return Task.FromResult(response);
        }
    }
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            //Debug.WriteLine(context.Exception);
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(new messageModel(bool.Parse(ConfigurationManager.AppSettings["showUnhandledError"]) ?
                (context.Exception.Message + " inner: " + (context.Exception.InnerException ?? new Exception("no inner exception")).Message) :
                "با عرض پوزش، درحال حاضر عملکرد برنامه با مشکل مواجه شده است").Jserialize()),
                ReasonPhrase = "Deadly Exception"
            });
        }
    }
    public class NotFoundActionResult : IHttpActionResult
    {
        private readonly string _message;
        private readonly HttpStatusCode _statusCode;

        public NotFoundActionResult(string message)
        {
            _statusCode = HttpStatusCode.NotFound;
            _message = new messageModel(message).Jserialize(); //string.Format("{{\"msg\" : \"{0}\"}}", message);

        }
        public NotFoundActionResult(object responseBody)
        {
            _statusCode = HttpStatusCode.NotFound;
            _message = responseBody.Jserialize(); //string.Format("{{\"msg\" : \"{0}\"}}", message);

        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_message)
            };
            return Task.FromResult(response);
        }
    }
    public class OKActionResultWithCustomHttpResponse : IHttpActionResult
    {


        private readonly HttpResponseMessage _responseBody;
        public OKActionResultWithCustomHttpResponse(HttpResponseMessage responseBody)
        {
            _responseBody = responseBody;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new StringContent(_message)
            //};
            return Task.FromResult(_responseBody);
        }
    }
    public class MultipleChoicesActionResult : IHttpActionResult
    {
        //private readonly string _message;
        private readonly HttpStatusCode _statusCode;
        private readonly object _responseBody;
        public MultipleChoicesActionResult(object responseBody)
        {
            _statusCode = HttpStatusCode.MultipleChoices;
            //_message = new messageModel(msg).Jserialize(); //string.Format("{{\"msg\" : \"{0}\"}}", message);
            _responseBody = responseBody;

        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_responseBody.Jserialize())
            };
            return Task.FromResult(response);
        }
    }


    public class ForbiddenActionResult : IHttpActionResult
    {
        private readonly string _message;
        private readonly HttpStatusCode _statusCode;

        public ForbiddenActionResult(string message = "access Denied")
        {
            _statusCode = HttpStatusCode.Forbidden;
            _message = string.Format("{{\"msg\" : \"{0}\"}}", message);
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_message)
            };
            return Task.FromResult(response);
        }
    }
    public class BadRequestActionResult : IHttpActionResult
    {
        private readonly string _message;
        private readonly HttpStatusCode _statusCode;

        public BadRequestActionResult(string message)
        {
            _statusCode = HttpStatusCode.BadRequest;
            _message = string.Format("{{\"msg\" : \"{0}\"}}", message);
        }
        public BadRequestActionResult(ICollection<ModelState> modelstateValues)
        {
            _statusCode = HttpStatusCode.BadRequest;
            _message = new messageModel(string.Join("\n", modelstateValues.SelectMany(v => v.Errors).Select(s => s.ErrorMessage))).Jserialize();
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_message)
            };
            return Task.FromResult(response);
        }
    }

    public class UnauthorizedActionResult : IHttpActionResult
    {
        private readonly string _message;
        private readonly HttpStatusCode _statusCode;

        public UnauthorizedActionResult(string message)
        {
            _statusCode = HttpStatusCode.Unauthorized;
            _message = new messageModel(message).Jserialize();
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_message)
            };
            return Task.FromResult(response);
        }
    }
    public class AutoActionResult<T> : IHttpActionResult
    {
        private readonly string _message;
        private readonly HttpStatusCode _statusCode;

        public AutoActionResult(int httpCode, string message, T value)
        {
            _statusCode = (HttpStatusCode)httpCode;
            _message = message;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_message),

            };
            return Task.FromResult(response);
        }
    }
    /// <summary>
    /// فقط برای لیستهایی از جنس کلاس استفاده شود
    /// </summary>
    public static class ClassToDatatableConvertor
    {
        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
    #endregion
    #region Extensions
    public static class Extensions
    {
        #region object

        public static string Jserialize(this object obj)
        {
            return new JavaScriptSerializer() { MaxJsonLength = int.MaxValue }.Serialize(obj);
        }
        public static object Jdeserialize(this string str, Type destinationTyp)
        {

            var js = new JavaScriptSerializer();
            return js.Deserialize(str, destinationTyp);
        }
        #endregion

        #region date time
        public static long ToUnixTimeStamp(this DateTime dt)
        {
            return (long)(TimeZoneInfo.ConvertTimeToUtc(dt) - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
        #endregion
        public static object getDbValue(this string val)
        {
            return string.IsNullOrEmpty(val) ? DBNull.Value : (object)(val
                .Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9")
                .Replace("ی", "ي").TrimStart().TrimEnd()
                // .Replace("ک", "ك").TrimStart().TrimEnd()

                );
        }
        public static object getDbValue(this int? val)
        {
            return val.HasValue ? (object)val.Value : DBNull.Value;
        }
        public static object getDbValue(this short? val)
        {
            return val.HasValue ? (object)val.Value : DBNull.Value;
        }
        public static object getDbValue(this DateTime? val)
        {
            return val.HasValue ? (object)val.Value : DBNull.Value;
        }
        public static object getDbValue(this long? val)
        {
            return val.HasValue ? (object)val.Value : DBNull.Value;
        }
        public static object getDbValue(this bool? val)
        {
            return val.HasValue ? (object)val.Value : DBNull.Value;
        }
        public static object getDbValue(this byte? val)
        {
            return val.HasValue ? (object)val.Value : DBNull.Value;
        }
        public static object getDbValue(this decimal? val)
        {
            return val.HasValue ? (object)val.Value : DBNull.Value;
        }

        public static object getDbValue(this double? val)
        {
            return val.HasValue ? (object)val.Value : DBNull.Value;
        }
        public static string dbNullCheckString(this object val)
        {
            return val is DBNull || val == null ? null : val.ToString();
        }
        public static long? dbNullCheckLong(this object val)
        {
            return val is DBNull ? new long?() : Convert.ToInt64(val);
        }
        public static byte? dbNullCheckByte(this object val)
        {
            return val is DBNull ? new byte?() : Convert.ToByte(val);
        }
        public static int? dbNullCheckInt(this object val)
        {
            return val is DBNull ? new int?() : Convert.ToInt32(val);
        }
        public static DateTime? dbNullCheckDatetime(this object val)
        {
            return val is DBNull ? new DateTime?() : Convert.ToDateTime(val);
        }
        public static double? dbNullCheckDouble(this object val)
        {
            return val is DBNull ? new double?() : Convert.ToDouble(val);
        }
        public static bool? dbNullCheckBoolean(this object val)
        {
            return val is DBNull ? new bool?() : Convert.ToBoolean(val);
        }
        public static decimal? dbNullCheckDecimal(this object val)
        {
            return val is DBNull ? new decimal?() : Convert.ToDecimal(val);
        }
        public static object getLongDbValue(this long? val)
        {
            return val.HasValue ? (object)val : DBNull.Value;
        }
        public static object getShortDbValue(this short? val)
        {
            return val.HasValue ? (object)val : DBNull.Value;
        }


    }

    public static class tool<T>
    {
        public static void setStringsToNullIfEmpty(ref T model)
        {

            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.PropertyType == typeof(string) && prop.GetValue(model) != null && string.Empty == prop.GetValue(model).ToString())
                    prop.SetValue(model, null);
            }

        }
    }
    #endregion
    #region common Models
    public class messageModel
    {
        public messageModel(string _msg)
        {
            msg = _msg;
        }
        public string msg { get; set; }
    }
    #endregion
}