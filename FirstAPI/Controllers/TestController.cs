using FirstAPI.DbContext;
using FirstAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace FirstAPI.Controllers
{
    public class TestController : ApiController
    {

        [System.Web.Http.HttpGet]
        public IEnumerable<TestModel> GetAll()
        {
            return new List<TestModel>() { new TestModel() { Name = "Cinna", TestId = 1 },new TestModel() { Name="Chypr",TestId=2 } };
        }

        [System.Web.Http.HttpGet]
        public IEnumerable<Player> GetAllPlayers()
        {
            var dal = new DAL();
            return dal.getAllPlayers();
        }
    }
}