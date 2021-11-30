using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Ct.Interview.Core;

namespace Ct.Interview.Service
{
    public partial class TradingContext : DbContext
    {
        public TradingContext()
        {

        }

        public TradingContext(DbContextOptions<TradingContext> options)
            : base(options)
        {

        }
        public virtual DbSet<Company> StockCompanies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("StockCompany");

                entity.Property(e => e.CompanyId).ValueGeneratedOnAdd();

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.ExchangeId)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.GicsIndustryGroup)
                    .HasMaxLength(100)
                    .HasColumnName("GICSIndustryGroup");

                entity.Property(e => e.StockCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.TradeStatus).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
