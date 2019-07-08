using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rayvarz.inv.assistant.service.Models
{
    /// <summary>
    /// مدل فیلتر عمومی
    /// </summary>
    
    public class publicFilterModel
    {
        /// <summary>
        /// گروه
        /// </summary>      
        public string group { get; set; }
        [JsonIgnore]
        internal int limit = 1000;//int.MaxValue
        /// <summary>
        /// کلید واژه
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// از کد
        /// </summary>
        [JsonIgnore]
        public int? _from { get; set; }
        /// <summary>
        /// از کد
        /// </summary>
        public string from
        {
            get
            {
                return _from.ToString();
            }
            set
            {
                int tmp;
                _from = string.IsNullOrEmpty(value) || !int.TryParse(value, out tmp) ? new int?() : tmp;
                //_from = string.IsNullOrEmpty(value) && !int.TryParse(value,) ? new int?() : int.Parse(value);
            }
        }
        /// <summary>
        /// تا کد
        /// </summary>
        [JsonIgnore]
        public int? _to { get; set; }
        /// <summary>
        /// تا کد
        /// </summary>
        public string to
        {
            get
            {
                return _to.ToString();
            }
            set
            {
                int tmp;
                _to = string.IsNullOrEmpty(value) || !int.TryParse(value, out tmp) ? new int?() : tmp;
                //_to = string.IsNullOrEmpty(value) ? new int?() : int.Parse(value);
            }
        }
        /// <summary>
        /// از ایندکس
        /// </summary>
        [JsonIgnore]
        public int _fromIndex { get; set; }
        /// <summary>
        /// تا ایندکس
        /// </summary>
        public string fromIndex
        {
            get
            {
                return _fromIndex.ToString();
            }
            set
            {
                int tmp;
                _fromIndex = string.IsNullOrEmpty(value) || !int.TryParse(value, out tmp) ? 0 : tmp;
                //_fromIndex = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
            }
        }
        /// <summary>
        /// تعداد خروجی
        /// </summary>
        [JsonIgnore]
        public int _take { get; set; }
        /// <summary>
        /// تعداد خروجی
        /// </summary>
        public string take
        {
            get
            {
                return _take.ToString();
            }
            set
            {
                int tmp;
                _take = string.IsNullOrEmpty(value) || !int.TryParse(value, out tmp) ? limit : tmp;
                //_take = string.IsNullOrEmpty(value) ? limit : int.Parse(value);
            }
        }

        /// <summary>
        /// عنوان فیلد مورد مرتب سازی
        /// </summary>
        public string orderBy { get; set; }
        /// <summary>
        /// مرتب سازی نزولی است؟
        /// </summary>
        public bool? isDescOrder { get; set; }
    }
}