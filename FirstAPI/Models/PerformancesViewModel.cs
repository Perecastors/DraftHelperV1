using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.Models
{
    public class PerformancesViewModel
    {
        public int championId { get; set; }
        public int enemyChampionId { get; set; }
        public int nbVictory { get; set; }
        public int nbDefeat { get; set; }
        public List<TimelineViewModel> timelines { get; set; }
        //public List<CreepsPerMinDeltasViewModel> listCreepsPerMinDeltasViewModel { get; set; }
        //public List<CsDiffPerMinDeltasViewModel> listCsPerMinDeltasViewModel { get; set; }
        //public List<GoldPerMinDeltasViewModel> listGoldPerMinDeltasViewModel { get; set; }
        //public List<CsDiffPerMinDeltasViewModel> listCsDiffPerMinDeltasViewModel { get; set; }
        //public List<XpDiffPerMinDeltasViewModel> listXpDiffPerMinDeltasViewModel { get; set; }
        //public List<DamageTakenPerMinDeltasViewModel> listDamageTakenPerMinDeltasViewModel { get; set; }
        //public List<DamageTakenDiffPerMinDeltasViewModel> listDamageTakenDiffPerMinDeltasViewModel { get; set; }

    }
}