using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.ApiServices
{

    public class Slots
    {
        public int id { get; set; }
        public string key { get; set; }
        public List<Rune> runes { get; set; } //leur json c'est de la merde,pas le choix de faire comme ca
    }

    public class AllRunes
    {
        public List<Slots> slots { get; set; }
    }


    public class AllRunesJson
    {
        public Slots[] slots { get; set; }
    }

    public class SlotsJson
    {
        public int id { get; set; }
        public string key { get; set; }
        public ARuneJson[] slots { get; set; } //leur json c'est de la merde,pas le choix de faire comme ca
    }

    public class ARuneJson
    {
        public Rune[] runes { get; set; }//leur json c'est de la merde,pas le choix de faire comme ca
    }

    public class Rune
    {
        public int id { get; set; }
        public string key { get; set; }
        public string icon { get; set; }
    }
}