﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MuzikosSistema.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MusicDBEntities : DbContext
    {
        public MusicDBEntities()
            : base("name=MusicDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Artist> Artist { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<ListenerProfile> ListenerProfile { get; set; }
        public virtual DbSet<ListenerRecomendation> ListenerRecomendation { get; set; }
        public virtual DbSet<ListenerStyles> ListenerStyles { get; set; }
        public virtual DbSet<Song> Song { get; set; }
        public virtual DbSet<SongArtist> SongArtist { get; set; }
        public virtual DbSet<SongLink> SongLink { get; set; }
        public virtual DbSet<SongLinkTypes> SongLinkTypes { get; set; }
        public virtual DbSet<SongRecomandation> SongRecomandation { get; set; }
        public virtual DbSet<Style> Style { get; set; }
        public virtual DbSet<SongArtistType> SongArtistType { get; set; }
    }
}
