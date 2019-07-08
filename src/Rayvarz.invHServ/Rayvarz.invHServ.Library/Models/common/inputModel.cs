using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rayvarz.invHServ.Library.Models.common
{
    //[DataContract]
    public class inputModel
    {
        //[DataMember]
        public string token { get; set; }
        //[DataMember]
        public string deviceId { get; set; }
        //[DataMember]
        //public string userName { get; set; }
        //[DataMember]
        //public short? Stype { get; set; }
        public inputModel()
        {
        }
        public inputModel(string _token)
        {
            token = _token;
        }
        public inputModel(string _token, string _DeviceId)
        {
            token = _token;
            deviceId = _DeviceId;
        }
        public inputModel(string _token, string _DeviceId, short _Stype)
        {
            token = _token;
            deviceId = _DeviceId;
            // Stype = _Stype;
        }
        public inputModel(string _token, string _DeviceId, short _Stype, int _dbCode)
        {
            token = _token;
            deviceId = _DeviceId;
            //Stype = _Stype;
            // dbCode = _dbCode;
        }
    }
    //[DataContract]
    public class inputModel<T>
    {
        //[DataMember]
        public string deviceId { get; set; }
        //[DataMember]
        //public string userName { get; set; }
        //[DataMember]
        //public short? Stype { get; set; }
        //[DataMember]
        public string token { get; set; }
        //[DataMember]
        public T value { get; set; }
        public inputModel()
        {
        }

        public inputModel(T _value)
        {
            value = _value;
        }
        public inputModel(string _deviceId, short _stype, string _token)
        {
            deviceId = _deviceId;
            // Stype = _stype;
            token = _token;
        }
        public inputModel(string _deviceId, short _stype, string _token, T _value)
        {
            deviceId = _deviceId;
            //  Stype = _stype;
            token = _token;
            value = _value;
        }
        public inputModel(string _deviceId, short _stype, string _token, T _value, int _dbCode)
        {
            deviceId = _deviceId;
            // Stype = _stype;
            token = _token;
            value = _value;
            //dbCode = _dbCode;
        }

    }
}