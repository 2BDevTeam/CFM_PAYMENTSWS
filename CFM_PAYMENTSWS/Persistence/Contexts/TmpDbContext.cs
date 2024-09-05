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

        public virtual DbSet<Liame> Liame { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=NACALADESENV;Database=OnBD_CFM_32;User Id=2badmin.imacinga;MultipleActiveResultSets=true;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Liame>(entity =>
            {
                entity.HasKey(e => e.Liamestamp);

                entity.ToTable("liame");

                entity.Property(e => e.Liamestamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("liamestamp")
                    .HasDefaultValueSql("(left(newid(),(25)))");

                entity.Property(e => e.Assunto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("assunto");

                entity.Property(e => e.Corpo)
                    .HasColumnType("text")
                    .HasColumnName("corpo");

                entity.Property(e => e.Keystamp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("keystamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ousrdata)
                    .HasColumnType("datetime")
                    .HasColumnName("ousrdata")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Para)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("para");

                entity.Property(e => e.Processado).HasColumnName("processado");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
