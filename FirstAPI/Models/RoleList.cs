using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.Models
{
    public enum roleList
    {
        Top = 1,
        Jungle = 2,
        Mid = 3,
        Adc = 4,
        Support = 5
    }

    public class roleClass
    {
        public int role { get; set; }
        public string roleName { get; set; }
    }
}