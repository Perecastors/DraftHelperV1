using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.ApiServices
{
    public class Account
    {
        public string id { get; set; }
        public string accountId { get; set; }
        public string name { get; set; }
        public long revisionDate { get; set; }
        public int summonerLevel { get; set; }
    }
}