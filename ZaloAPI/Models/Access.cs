using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZaloAPI.Models
{
    public class Access
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
}