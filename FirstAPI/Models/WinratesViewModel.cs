using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.Models
{
    public class WinratesViewModel
    {
        public int totalGames { get; set; }
        public double totalWinrate { get; set; }
        public Side blueSide { get; set; }
        public Side redSide { get; set; }

        public class Side
        {
            public int totalGames { get; set; }
            public int nbWin { get; set; }
            public int nbLoss { get; set; }
        }

    }
}