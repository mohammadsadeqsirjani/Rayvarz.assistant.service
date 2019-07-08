using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Rayvarz.invHServ.Api.Models.Common
{
    public class publicFilterModel
    {
        [IgnoreDataMember]
        internal int limit = 1000;//int.MaxValue
        public string key { get; set; }
        [IgnoreDataMember]
        public int? _from { get; set; }
        public string from
        {
            get
            {
                return _from.ToString();
            }
            set
            {
                _from = string.IsNullOrEmpty(value) ? new int?() : int.Parse(value);
            }
        }
        [IgnoreDataMember]
        public int? _to { get; set; }
        public string to
        {
            get
            {
                return _to.ToString();
            }
            set
            {
                _to = string.IsNullOrEmpty(value) ? new int?() : int.Parse(value);
            }
        }
        [IgnoreDataMember]
        public int _fromIndex { get; set; }
        public string fromIndex
        {
            get
            {
                return _fromIndex.ToString();
            }
            set
            {
                _fromIndex = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
            }
        }
        [IgnoreDataMember]
        public int _take { get; set; }
        public string take
        {
            get
            {
                return _take.ToString();
            }
            set
            {
                _take = string.IsNullOrEmpty(value) ? limit : int.Parse(value);
            }
        }
    }
}