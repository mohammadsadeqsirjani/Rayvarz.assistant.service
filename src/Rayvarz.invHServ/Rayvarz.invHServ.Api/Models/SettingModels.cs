using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Web;

namespace Rayvarz.invHServ.Api.Models
{
    [Table("AppSettings")]
    public class SettingModels
    {
        [Key]
        public long ID { get; set; }
        public string v1 { get; set; }
        public string v2 { get; set; }
        public string v3 { get; set; }
        public string v4 { get; set; }
    }
}