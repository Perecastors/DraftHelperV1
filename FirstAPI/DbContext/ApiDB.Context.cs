﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Database1Entities : DbContext
    {
        public Database1Entities()
            : base("name=Database1Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ChampionPool> ChampionPools { get; set; }
        public virtual DbSet<Champion> Champions { get; set; }
        public virtual DbSet<ChampionTag> ChampionTags { get; set; }
        public virtual DbSet<MatchupComment> MatchupComments { get; set; }
        public virtual DbSet<MatchupRespons> MatchupResponses { get; set; }
        public virtual DbSet<Matchup> Matchups { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
