﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eus.UI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class eusVilleEntities : DbContext
    {
        public eusVilleEntities()
            : base("name=eusVilleEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cork_HousingType> Cork_HousingType { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Cork_Category> Cork_Category { get; set; }
        public virtual DbSet<Cork_PostHousing> Cork_PostHousing { get; set; }
    
        public virtual int GetCategories()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetCategories");
        }
    
        public virtual int GetHousingType()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetHousingType");
        }
    }
}