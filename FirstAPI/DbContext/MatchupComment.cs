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
    
    public partial class MatchupComment
    {
        public System.Guid MatchupCommentId { get; set; }
        public System.Guid PlayerId { get; set; }
        public System.Guid MatchupId { get; set; }
        public System.Guid ChampionId { get; set; }
        public string CommentText { get; set; }
        public System.DateTime CreationDate { get; set; }
    }
}