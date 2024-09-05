using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CFM_PAYMENTSWS.Domains.Models;

namespace CFM_PAYMENTSWS.Persistence.Contexts
{
    public partial class TmpDbContext : DbContext
    {
        public TmpDbContext()
        {
        }

        public TmpDbContext(DbContextOptions<TmpDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Suliame> Suliame { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=CFM-PMO-RPT\\EXP2019;Database=OnBD_WS;User Id=ws;password=12345678Ab!;MultipleActiveResultSets=true;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Suliame>(entity =>
            {
                entity.HasKey(e => e.Userno);

                entity.ToTable("suliame");

                entity.Property(e => e.Userno)
                    .HasColumnType("decimal(12, 0)")
                    .HasColumnName("userno");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tlmvl)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tlmvl")
                    .HasDefaultValueSql("('')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
