using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.Models
{
    public class ChampionSpellInfosViewModel
    {
        public string id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public List<Spells> spells { get; set; }
    }

    public class Spells
    {
        public string cooldownBurn { get; set; }
        public string name { get; set; }
        public string tooltip { get; set; }
        public decimal[] cooldown { get; set; }
        public Image image { get; set; }
    }

    public class Image
    {
        public string full { get; set; }
        public string sprite { get; set; }
    }

}