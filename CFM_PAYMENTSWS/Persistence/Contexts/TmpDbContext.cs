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

        public virtual DbSet<Tb> Tb { get; set; } = null!;

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
            modelBuilder.Entity<Tb>(entity =>
            {
                entity.HasKey(e => e.Tbstamp)
                    .HasName("pk_tb")
                    .IsClustered(false);

                entity.ToTable("tb");

                entity.HasIndex(e => e.Cheque, "in_tb_cheque")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Data, "in_tb_data")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Descricao, "in_tb_descricao")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Documento, "in_tb_documento")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.Data, e.Documento, e.Cheque, e.Descricao, e.Evalor, e.Valor, e.Ollocal, e.Tbstamp }, "in_tb_tblist")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Tbno, "in_tb_tbno")
                    .HasFillFactor(80);

                entity.Property(e => e.Tbstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("tbstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Cbbstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cbbstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ccusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ccusto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cheque)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("cheque")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Contado)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("contado");

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("data")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("descricao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Diaplano)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("diaplano");

                entity.Property(e => e.Dilnoplano)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("dilnoplano");

                entity.Property(e => e.Dinoplano)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("dinoplano")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Documento)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("documento")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Dplano)
                    .HasColumnType("datetime")
                    .HasColumnName("dplano")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Evalor)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evalor");

                entity.Property(e => e.Filestatus)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("filestatus")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Filestatusdate)
                    .HasColumnType("datetime")
                    .HasColumnName("filestatusdate")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Formatoexp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("formatoexp");

                entity.Property(e => e.Fref)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("fref")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Frreford)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("frreford")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Grupo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("grupo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Idficheiro)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("idficheiro")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Intid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("intid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Local)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("local")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Ncusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ncusto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Olbancos).HasColumnName("olbancos");

                entity.Property(e => e.Olcodigo)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("olcodigo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ollocal)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ollocal")
                    .HasDefaultValueSql("('')");

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

                entity.Property(e => e.Plano).HasColumnName("plano");

                entity.Property(e => e.Prreford)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("prreford")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Sgrupo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("sgrupo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Sr).HasColumnName("sr");

                entity.Property(e => e.Tbid)
                    .HasColumnType("numeric(12, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("tbid");

                entity.Property(e => e.Tbno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("tbno");

                entity.Property(e => e.UDatatrf)
                    .HasColumnType("datetime")
                    .HasColumnName("u_datatrf")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.UOriid)
                    .HasColumnType("numeric(9, 0)")
                    .HasColumnName("u_oriid");

                entity.Property(e => e.USendpay).HasColumnName("u_sendpay");

                entity.Property(e => e.UUsrtrf)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("u_usrtrf")
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

                entity.Property(e => e.Valor)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("valor");

                entity.Property(e => e.Viabanco)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("viabanco");

                entity.Property(e => e.Vrreford)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("vrreford")
                    .HasDefaultValueSql("('')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
