//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FirstAPI.DbContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class Matchup
    {
        public System.Guid MatchupId { get; set; }
        public System.Guid PlayerId { get; set; }
        public System.Guid MatchupResponseId { get; set; }
        public Nullable<System.Guid> EnemyTop { get; set; }
        public Nullable<System.Guid> EnemyJungle { get; set; }
        public Nullable<System.Guid> EnemyMid { get; set; }
        public Nullable<System.Guid> EnemyAdc { get; set; }
        public Nullable<System.Guid> EnemySupport { get; set; }
        public Nullable<System.Guid> AllyTop { get; set; }
        public Nullable<System.Guid> AllyJungle { get; set; }
        public Nullable<System.Guid> AllyMid { get; set; }
        public Nullable<System.Guid> AllyAdc { get; set; }
        public Nullable<System.Guid> AllySupport { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string PatchVersion { get; set; }
    }
}
