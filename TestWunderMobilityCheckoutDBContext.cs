﻿using Communication.EventDataService;
using Microsoft.EntityFrameworkCore;

namespace TestWunderMobilityCheckout
{
    /// <summary> DBContext for authorization and authentication </summary>
    public class TestWunderMobilityCheckoutDBContext : DbContext
    {
        /// <summary> Constructor </summary>
        /// <param name="options"> Options</param>
        public TestWunderMobilityCheckoutDBContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary> Table to work with events (client part) </summary>
        public DbSet<EventDataModel> EventDataModel { get; set; }

        /// <summary> Change default table names so on </summary>
        /// <param name="modelBuilder"> Model builder </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Events
            modelBuilder.ApplyConfiguration(new EventDataModelConfig());
        }
    }
}
