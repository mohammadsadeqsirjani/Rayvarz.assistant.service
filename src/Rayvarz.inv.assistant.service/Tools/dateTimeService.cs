using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Rayvarz.inv.assistant.service.Tools
{
    public static class dateTimeService
    {
        public static string crntMiladiDate
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMdd");
            }
        }
        /// <summary>
        /// returns current persian date in Rayvarz format like : 13950103
        /// </summary>
        public static string crntPersianDate
        {
            get
            {
                var pc = new PersianCalendar();
                return pc.GetYear(DateTime.Now).ToString() + pc.GetMonth(DateTime.Now).ToString("D2") + pc.GetDayOfMonth(DateTime.Now).ToString("D2");

            }
        }
        public static string ToPersianDateTime(this DateTime date)
        {
            var pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2} {3}", pc.GetYear(date), pc.GetMonth(date).ToString("D2"), pc.GetDayOfMonth(date).ToString("D2"), date.ToString("HH:mm:ss"));
        }
        public static string toMiladiDate(this string persianDate)
        {
            var pc = new PersianCalendar();
            try
            {
                return new DateTime(int.Parse(persianDate.Substring(0, 4)), int.Parse(persianDate.Substring(4, 2)), int.Parse(persianDate.Substring(6, 2)), pc).ToString("yyyyMMdd");
            }
            catch { return string.Empty; }
        }
        public static DateTime ToDateTime(this string date)
        {
            return DateTime.Parse(date);
        }
        //private string GetPersianDate(DateTime dt)
        //{
        //    var pc = new PersianCalendar();
        //    try
        //    {
        //        return pc.GetYear(dt).ToString() + pc.GetMonth(dt).ToString("D2") + pc.GetDayOfMonth(dt).ToString("D2");
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}
        //private string GetPersianDate(string dtString)
        //{
        //    try
        //    {
        //        DateTime dt = DateTime.Parse(dtString);
        //        var pc = new PersianCalendar();
        //        return pc.GetYear(dt).ToString() + pc.GetMonth(dt).ToString("D2") + pc.GetDayOfMonth(dt).ToString("D2");
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}
        //private string GetTimeString(DateTime dt)
        //{
        //    try
        //    {
        //        return dt.ToString("HH:mm");
        //    }
        //    catch { return string.Empty; }
        //}
        //private string GetTimeString(string dtString)
        //{
        //    try
        //    {
        //        DateTime dt = DateTime.Parse(dtString);
        //        return dt.ToString("HH:mm");
        //    }
        //    catch { return string.Empty; }
        //}
    }
}