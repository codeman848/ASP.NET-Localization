using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEC06.Models.ViewModels
{
    public class forUserList
    {
        public string userId { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public Boolean locked { get; set; }
        public Boolean external { get; set; }
        public Boolean memberOfAdministrators { get; set; }

    }
}