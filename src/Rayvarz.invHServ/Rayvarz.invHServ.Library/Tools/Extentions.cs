using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Rayvarz.invHSyncServ.Library.Tools
{
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

        public static T FillTo<T>(this object From, ref object To)
        {
            foreach (var Param in To.GetType().GetProperties().ToList())
            {
                try
                {
                    To.GetType().GetProperty(Param.Name).SetValue(To,
                        From.GetType().GetProperty(Param.Name).GetValue(From, null), null);

                }
                catch {/*If Property Not Exists Dont Return Error*/ }
            }

            return (T)To;
        }

        public static T FillFrom<T>(this object From, object To)
        {
            foreach (var Param in To.GetType().GetProperties().ToList())
            {
                try
                {
                    To.GetType().GetProperty(Param.Name).SetValue(To,
                        From.GetType().GetProperty(Param.Name).GetValue(From, null), null);

                }
                catch {/*If Property Not Exists Dont Return Error*/ }
            }

            return (T)To;
        }
        #endregion object

        #region chars , strrings

        public static string CorrectName(this string name)
        {
            if (string.IsNullOrEmpty(name))
                return "کاربر";
            name = name.Replace('ي', 'ی');
            name = name.ToLower();
            name = name.Trim();
            #region remove Prefix , Suffix
            name = (name.StartsWith("سیده") && !string.IsNullOrEmpty(name.Replace("سیده", "").Trim())) ? name.Replace("سیده", "").Trim() : name;
            name = (name.StartsWith("seyedeh") && !string.IsNullOrEmpty(name.Replace("seyedeh", "").Trim())) ? name.Replace("seyedeh", "").Trim() : name;
            name = (name.StartsWith("seyede") && !string.IsNullOrEmpty(name.Replace("seyede", "").Trim())) ? name.Replace("seyede", "").Trim() : name;
            name = (name.StartsWith("سید") && !string.IsNullOrEmpty(name.Replace("سید", "").Trim())) ? name.Replace("سید", "").Trim() : name;
            name = (name.StartsWith("seyed") && !string.IsNullOrEmpty(name.Replace("seyed", "").Trim())) ? name.Replace("seyed", "").Trim() : name;
            name = (name.StartsWith("sed ") && !string.IsNullOrEmpty(name.Replace("sed ", "").Trim())) ? name.Replace("sed ", "").Trim() : name;
            name = (name.Contains("سادات") && !string.IsNullOrEmpty(name.Replace("سادات", "").Trim())) ? name.Replace("سادات", "").Trim() : name;
            name = (name.Contains("sadat") && !string.IsNullOrEmpty(name.Replace("sadat", "").Trim())) ? name.Replace("sadat", "").Trim() : name;
            #endregion
            if (name.ToCharArray().Count() > 15)
                name = name.Substring(0, 15);
            return (new char[] { 'ا', 'آ', 'و' }.Contains(name.GetLast(1).ToCharArray()[0]) ?
                name + "ی" : new char[] { 'ه', 'i', 'I', 'e', 'E', 'a', 'A', 'ی', 'ي', 'o', 'O', 'u', 'U' }.Contains(name.GetLast(1).ToCharArray()[0]) && name.GetLast(4) != "الله" && name.GetLast(4) != " اله" && name.GetLast(3) != "اله" && name.GetLast(2) != "اه" && name.GetLast(2) != "آه" /*&& name.GetLast(2) != "به"*/ ? name + " ی" :
                new char[] { 'h', 'H' }.Contains(name.GetLast(1).ToCharArray()[0]) && name.GetLast(2) != "sh" && name.GetLast(2) != "ah" && name.GetLast(3) != "beh" ? name + " ی" : name
                ).Trim();
        }



        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }


        static char[][] numbers = new char[][]
        {
            "0123456789".ToCharArray(),"۰۱۲۳۴۵۶۷۸۹".ToCharArray()
        };
        static char[][] alphebets = new char[][]
        {
            "SNZMQTYLFJ".ToCharArray(),"0123456789".ToCharArray()
        };
        public static string toPrevalentDigits(this string src)
        {
            if (string.IsNullOrEmpty(src)) return null;
            for (int x = 0; x <= 9; x++)
            {
                src = src.Replace(numbers[1][x], numbers[0][x]);
            }
            return src;
        }
        public static string mapToAlphebet(this string src)
        {
            if (string.IsNullOrEmpty(src)) return null;
            for (int x = 0; x <= 9; x++)
            {
                src = src.Replace(alphebets[1][x], alphebets[0][x]);
            }
            return src;
        }
        public static string mapToDigit(this string src)
        {
            if (string.IsNullOrEmpty(src)) return null;
            for (int x = 0; x <= 9; x++)
            {
                src = src.Replace(alphebets[0][x], alphebets[1][x]);
            }
            return src;
        }
        public static string toPersianDigits(this string src)
        {
            if (string.IsNullOrEmpty(src)) return null;
            for (int x = 0; x <= 9; x++)
            {
                src = src.Replace(numbers[0][x], numbers[1][x]);
            }
            return src;
        }
        #endregion

        //#region portal
        //public static MvcHtmlString _toHtmlString(this string input)
        //{
        //    return new MvcHtmlString(input);
        //}

        //public static string _toString(this ICollection<ModelState> sate)
        //{
        //    string errorMessage = String.Empty;
        //    foreach (ModelState modelState in sate)
        //    {
        //        foreach (ModelError error in modelState.Errors)
        //        {
        //            errorMessage += error.ErrorMessage + " </br> ";
        //        }
        //    }
        //    return errorMessage;
        //}
        //#endregion

        #region dateTime
        /// <summary>
        /// returns current persian date in Rayvarz format like : 13950103
        /// </summary>
        public static string ToRvzPersianDate(this DateTime dt)
        {

            var pc = new PersianCalendar();
            return pc.GetYear(dt).ToString() + pc.GetMonth(dt).ToString("D2") + pc.GetDayOfMonth(dt).ToString("D2");
        }
        public static string ToPersianDate(this DateTime date)
        {
            var pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2}", pc.GetYear(date), pc.GetMonth(date).ToString("D2"), pc.GetDayOfMonth(date).ToString("D2"));
        }
        public static DateTime ToMiladiDateTime(this string date)
        {

            try
            {
                date = date.Replace("/", "");
                date = date.Replace(@"\", "");
                if (date.Length != 8 || !date.ToCharArray().All(c => char.IsDigit(c)))
                    throw new Exception("invalid input date");
                //string[] date = //jalaliDate.Split('/');
                int year = int.Parse(date.Substring(0, 4));  //Convert.ToInt32(date[0]);
                int month = int.Parse(date.Substring(4, 2)); //Convert.ToInt32(date[1]);
                int day = int.Parse(date.Substring(6, 2)); //Convert.ToInt32(date[2]);
                //return new DateTime(0,0,0,,,,,)
                return new PersianCalendar().ToDateTime(year, month, day, 0, 0, 0, 0, PersianCalendar.PersianEra);
            }
            catch
            {
                throw new FormatException("The input string must be in 0000/00/00 format.");
            }
        }

        public static DateTime ToMiladiEndDayDateTime(this string date)
        {

            try
            {
                date = date.Replace("/", "");
                date = date.Replace(@"\", "");
                if (date.Length != 8 || !date.ToCharArray().All(c => char.IsDigit(c)))
                    throw new Exception("invalid input date");
                //string[] date = //jalaliDate.Split('/');
                int year = int.Parse(date.Substring(0, 4));  //Convert.ToInt32(date[0]);
                int month = int.Parse(date.Substring(4, 2)); //Convert.ToInt32(date[1]);
                int day = int.Parse(date.Substring(6, 2)); //Convert.ToInt32(date[2]);
                //return new DateTime(0,0,0,,,,,)
                return new PersianCalendar().ToDateTime(year, month, day, 23, 59, 59, 999, PersianCalendar.PersianEra);
            }
            catch
            {
                throw new FormatException("The input string must be in 0000/00/00 format.");
            }
        }
        //How to use it:
        //string pdate = "1392/02/31";
        //DateTime dateFromJalali = pdate.GetDateTimeFromJalaliString(); //{5/21/2014 12:00:00 AM}
        public static string GetDayOfWeekName(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday: return "شنبه";
                case DayOfWeek.Sunday: return "يکشنبه";
                case DayOfWeek.Monday: return "دوشنبه";
                case DayOfWeek.Tuesday: return "سه‏ شنبه";
                case DayOfWeek.Wednesday: return "چهارشنبه";
                case DayOfWeek.Thursday: return "پنجشنبه";
                case DayOfWeek.Friday: return "جمعه";
                default: return "";
            }
        }
        //How to use it:
        //DateTime date = DateTime.Now;
        //string wname = date.GetDayOfWeekName();
        public static string GetMonthName(this DateTime date)
        {
            PersianCalendar jc = new PersianCalendar();
            string pdate = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));

            string[] dates = pdate.Split('/');
            int month = Convert.ToInt32(dates[1]);

            switch (month)
            {
                case 1: return "فررودين";
                case 2: return "ارديبهشت";
                case 3: return "خرداد";
                case 4: return "تير‏";
                case 5: return "مرداد";
                case 6: return "شهريور";
                case 7: return "مهر";
                case 8: return "آبان";
                case 9: return "آذر";
                case 10: return "دي";
                case 11: return "بهمن";
                case 12: return "اسفند";
                default: return "";
            }

        }
        public static string GetMonthShamsiName(this DateTime date)
        {
            int month = date.Month;

            switch (month)
            {
                case 1: return "فررودين";
                case 2: return "ارديبهشت";
                case 3: return "خرداد";
                case 4: return "تير‏";
                case 5: return "مرداد";
                case 6: return "شهريور";
                case 7: return "مهر";
                case 8: return "آبان";
                case 9: return "آذر";
                case 10: return "دي";
                case 11: return "بهمن";
                case 12: return "اسفند";
                default: return "";
            }

        }
        public static string GetMonthNameByNum(this string date)
        {
            int month = Convert.ToInt16(date);

            switch (month)
            {
                case 1: return "فررودين";
                case 2: return "ارديبهشت";
                case 3: return "خرداد";
                case 4: return "تير‏";
                case 5: return "مرداد";
                case 6: return "شهريور";
                case 7: return "مهر";
                case 8: return "آبان";
                case 9: return "آذر";
                case 10: return "دي";
                case 11: return "بهمن";
                case 12: return "اسفند";
                default: return "";
            }

        }
        //How to use it:
        //DateTime date = DateTime.Now;
        //string mname = date.GetMonthName();

        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            PersianCalendar jc = new PersianCalendar();
            string pdate = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));

            string[] dates = pdate.Split('/');
            int year = Convert.ToInt16(dates[0]);
            int month = Convert.ToInt16(dates[1]);
            DateTime dateTime = new DateTime(year, month, 01, 0, 0, 0);

            return dateTime;
        }

        public static string AddSlashesToPersianDate(this string Date)
        {
            return string.IsNullOrEmpty(Date) ? null : Date.Substring(0, 4) + "/" + Date.Substring(4, 2) + "/" + Date.Substring(6, 2);
        }
        public static DateTime ToDateTime(this long unixTimeStamp)
        {
            try
            {
                // Unix timestamp is seconds past epoch
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
                return dtDateTime;
            }
            catch { return new DateTime(); }
        }

        public static long ToUnixTimeStamp(this DateTime dt)
        {
            return (long)(TimeZoneInfo.ConvertTimeToUtc(dt) - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
        public static string ToPersianDateTime(this DateTime date)
        {
            var pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2} {3}", pc.GetYear(date), pc.GetMonth(date).ToString("D2"), pc.GetDayOfMonth(date).ToString("D2"), date.ToString("HH:mm:ss"));
        }



        public static string TofullPersianDatetimeString(this DateTime dt)
        {
            try
            {
                return string.Format("{0} {1} {2} {4} ، ساعت {3}", dt.GetDayOfWeekName(), new PersianCalendar().GetDayOfMonth(dt), dt.GetMonthName(), dt.ToString("HH:mm:ss tt"), new PersianCalendar().GetYear(dt)).Replace("PM", "(بعد از ظهر)").Replace("AM", "(قبل از ظهر)");
            }
            catch { return "-نامعلوم-"; }
        }

        #endregion

        #region concurency
        public static decimal roundTo500Rials(this decimal val)
        {
            decimal roundedDecimals = decimal.Round(val, 0);
            return roundedDecimals % 500 <= 250 ? roundedDecimals - (roundedDecimals % 500) : roundedDecimals + (500 - (roundedDecimals % 500));
        }
        public static decimal roundToRials(this decimal val)
        {
            decimal roundedDecimals = decimal.Round(val, 0);
            return roundedDecimals;
        }
        public static decimal toToman(this decimal val)
        {
            return val / 10;
        }

        public static string toToman_AsdisplayValue(this decimal val)
        {
            return string.Format("{0} تومان", val.toToman().ToString("N0"));
        }

        public static string toDisplayValueAsToman(this decimal val)
        {
            return string.Format("{0} تومان", val.ToString("N0"));
        }

        #endregion
    }
}
