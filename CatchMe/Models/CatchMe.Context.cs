﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CatchMe.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CatchMeDBEntities : DbContext
    {
        public CatchMeDBEntities()
            : base("name=CatchMeDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<project> projects { get; set; }
        public virtual DbSet<task> tasks { get; set; }
        public virtual DbSet<taskUser> taskUsers { get; set; }
        public virtual DbSet<employee> employees { get; set; }
        public virtual DbSet<comment> comments { get; set; }
        public virtual DbSet<projectUser> projectUsers { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<log> logs { get; set; }
    }
}
