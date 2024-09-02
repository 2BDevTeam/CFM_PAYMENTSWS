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

        public virtual DbSet<Qu> Qu { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=SRV05\\SQLDEV2019;Database=onbd_HPB;User Id=isac.munguambe;password=Murd3rB4nd;MultipleActiveResultSets=true;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

            modelBuilder.Entity<Qu>(entity =>
            {
                entity.HasKey(e => e.Quarto)
                    .HasName("pk_qu")
                    .IsClustered(false);

                entity.ToTable("qu");

                entity.HasIndex(e => e.Bloqueado, "in_qu_qubloq")
                    .HasFillFactor(70);

                entity.HasIndex(e => new { e.Tipo, e.Quarto, e.Ref, e.Bloqueado, e.Qustamp }, "in_qu_qulist")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Quarto, "in_qu_ququarto")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Ref, "in_qu_quref")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Tipo, "in_qu_qutipo")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Qustamp, "in_qu_stamp")
                    .IsUnique()
                    .HasFillFactor(70);

                entity.Property(e => e.Quarto)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("quarto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Bloqueado).HasColumnName("bloqueado");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Motivo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("motivo")
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

                entity.Property(e => e.Qustamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("qustamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Ref)
                    .HasMaxLength(18)
                    .IsUnicode(false)
                    .HasColumnName("ref")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("tipo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UIdserv)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_idserv")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UPiso)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("u_piso");

                entity.Property(e => e.UPorhora).HasColumnName("u_porhora");

                entity.Property(e => e.UServico)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("u_servico")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UUnidade)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("u_unidade")
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
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
