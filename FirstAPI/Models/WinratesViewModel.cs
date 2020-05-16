using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.Models
{
    public class WinratesViewModel
    {
        public int totalGamesOnlyMainRole { get; set; }
        public int totalGames { get; set; }
        public double totalWinrate { get; set; }
        public Side blueSide { get; set; }
        public Side redSide { get; set; }
        public string role { get; set; }

        public class Side
        {
            public int totalGames { get; set; }
            public int nbWin { get; set; }
            public int nbLoss { get; set; }
            public int nbPick1 { get; set; }
            public int nbPick2 { get; set; }
            public int nbPick3 { get; set; }
            public int nbPick4 { get; set; }
            public int nbPick5 { get; set; }
            public int nbPick6 { get; set; }
            public int nbPick7 { get; set; }
            public int nbPick8 { get; set; }
            public int nbPick9 { get; set; }
            public int nbPick10 { get; set; }

            public int nbWinPick1 { get; set; }
            public int nbWinPick2 { get; set; }
            public int nbWinPick3 { get; set; }
            public int nbWinPick4 { get; set; }
            public int nbWinPick5 { get; set; }
            public int nbWinPick6 { get; set; }
            public int nbWinPick7 { get; set; }
            public int nbWinPick8 { get; set; }
            public int nbWinPick9 { get; set; }
            public int nbWinPick10 { get; set; }
        }

    }
}