using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rayvarz.invHServ.Library.Models.common
{
    public class resultModel
    {
        public resultModel()
        {
        }
        public resultModel(bool _result)
        {
            result = _result;
            if (result)
                message = "$$" + "تراکنش با موفقیت انجام شد";
            else
                message = "$$" + "تراکنش با شکست مواجه شد";

        }
        public resultModel(bool result, string message)
        {
            this.result = result;
            this.message = message;
        }
        public resultModel(Exception err)
        {
            result = false;
            message = err.Message;
        }
        //[DataMember]
        public bool result { get; set; }
        //[DataMember]
        public string message { get; set; }
    }
    //[DataContract]
    public class resultModel<T>
    {
        private string SucceedMsg = "$$" + "تراکنش با موفقیت انجام شد";
        private string FaultMsg = "$$" + "تراکنش با شکست مواجه شد";
        //[DataMember]
        public bool result { get; set; }
        //[DataMember]
        public string message { get; set; }
        //[DataMember]
        public T value { get; set; }

        public resultModel() { }

        public resultModel(bool result, string message)
        {
            this.result = result;
            this.message = message;
        }

        public resultModel(bool result, string message, T value)
        {
            this.result = result;
            this.message = message;
            this.value = value;
        }
        public resultModel(bool result, T value)
        {
            this.result = result;
            if (result)
                message = SucceedMsg;
            else
                message = FaultMsg;
            this.value = value;
        }

        public resultModel(T value)
        {
            result = true;
            message = "$$" + "تراکنش انجام شد.";
            this.value = value;
        }

        public resultModel(Exception err)
        {
            result = false;
            message = err.Message;
        }

        public void raiseErrorIfResultFalse()
        {
            if (!result)
                throw new Exception(message);
        }
    }
}