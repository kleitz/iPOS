﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iPOS.Web.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbpawnshopEntities : DbContext
    {
        public dbpawnshopEntities()
            : base("name=dbpawnshopEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<customer> customers { get; set; }
        public virtual DbSet<tbldepartment> tbldepartments { get; set; }
        public virtual DbSet<tblrole> tblroles { get; set; }
        public virtual DbSet<tbluser> tblusers { get; set; }
        public virtual DbSet<itemcategory> itemcategories { get; set; }
        public virtual DbSet<itemtype> itemtypes { get; set; }
        public virtual DbSet<releaseditem> releaseditems { get; set; }
        public virtual DbSet<apraiseditem> apraiseditems { get; set; }
        public virtual DbSet<pawneditem> pawneditems { get; set; }
    }
}
