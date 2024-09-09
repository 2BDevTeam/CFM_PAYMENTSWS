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

        public virtual DbSet<UTrfb> UTrfb { get; set; } = null!;

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
            modelBuilder.Entity<UTrfb>(entity =>
            {
                entity.HasKey(e => e.UTrfbstamp)
                    .HasName("pk_u_trfb")
                    .IsClustered(false);

                entity.ToTable("u_trfb");

                entity.Property(e => e.UTrfbstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_trfbstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Adito).HasColumnName("adito");

                entity.Property(e => e.Banco)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("banco")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Bc)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("bc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cct)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cct")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ccusto)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ccusto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Corrente).HasColumnName("corrente");

                entity.Property(e => e.Datatrf)
                    .HasColumnType("datetime")
                    .HasColumnName("datatrf")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Docta).HasColumnName("docta");

                entity.Property(e => e.Dt)
                    .HasColumnType("datetime")
                    .HasColumnName("dt")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Dtf)
                    .HasColumnType("datetime")
                    .HasColumnName("dtf")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Dti)
                    .HasColumnType("datetime")
                    .HasColumnName("dti")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Ficheiro)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ficheiro")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Formatrf)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("formatrf")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Id)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("id")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.No)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("no");

                entity.Property(e => e.Ousrdata)
                    .HasColumnType("datetime")
                    .HasColumnName("ousrdata")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ousrhora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ousrhora")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ousrinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ousrinis")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Pagto).HasColumnName("pagto");

                entity.Property(e => e.Qtd)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("qtd");

                entity.Property(e => e.Rdata)
                    .HasColumnType("datetime")
                    .HasColumnName("rdata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Rno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("rno");

                entity.Property(e => e.Sendpay).HasColumnName("sendpay");

                entity.Property(e => e.Stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("stamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Usrdata)
                    .HasColumnType("datetime")
                    .HasColumnName("usrdata")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Usrhora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("usrhora")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Usrinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("usrinis")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Usrtrf)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("usrtrf")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Valor)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("valor");

                entity.Property(e => e.Valortrf)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("valortrf");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
