﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using talabat.core.Entites;
using talabat.core.Entites.Order_Aggregate;
using talabat.Repository.Data.Config;

namespace talabat.Repository
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext>options) : base (options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ///modelBuilder.ApplyConfiguration(new ProductBrandConfegrations());
            ///modelBuilder.ApplyConfiguration(new ProductTypeConfigurations());
            ///modelBuilder.ApplyConfiguration(new ProductConfigrations());


            // this line make the same object that the before 3 lines make it
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<Order> orders  { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }
        public DbSet<DeliveryMethod> deliveryMethods { get; set; }
    }
}
