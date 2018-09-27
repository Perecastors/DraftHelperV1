using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace FirstAPI.DbContext
{
    public partial class Database1Entities
    {
        public Database1Entities(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }
    }

}