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

        public virtual DbSet<U2bPaymentsQueueTs> U2bPaymentsQueueTs { get; set; } = null!;

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
            modelBuilder.Entity<U2bPaymentsQueueTs>(entity =>
            {
                entity.HasKey(e => e.U2bPaymentsQueueTsstamp)
                    .HasName("pk_u_2b_paymentsQueue_ts")
                    .IsClustered(false);

                entity.ToTable("u_2b_paymentsQueue_ts");

                entity.Property(e => e.U2bPaymentsQueueTsstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_2b_paymentsQueue_tsstamp");

                entity.Property(e => e.BatchId)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("batchID")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BeneficiaryName)
                    .HasColumnType("text")
                    .HasColumnName("beneficiaryName");

                entity.Property(e => e.Canal).HasColumnName("canal");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descricao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasColumnName("description")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Destino)
                    .IsUnicode(false)
                    .HasColumnName("destino")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Docno)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("docno");

                entity.Property(e => e.Emailf)
                    .HasColumnType("text")
                    .HasColumnName("emailf");

                entity.Property(e => e.Estado)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Keystamp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("keystamp");

                entity.Property(e => e.Lordem)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("lordem");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Moeda)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("moeda")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Origem)
                    .IsUnicode(false)
                    .HasColumnName("origem")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Oristamp)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("oristamp");

                entity.Property(e => e.Ousrdata)
                    .HasColumnType("date")
                    .HasColumnName("ousrdata")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ousrhora)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ousrhora")
                    .HasDefaultValueSql("(left(CONVERT([time],getdate()),(8)))");

                entity.Property(e => e.Ousrinis)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ousrinis")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ProcessingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("processingDate");

                entity.Property(e => e.Tabela)
                    .IsUnicode(false)
                    .HasColumnName("tabela")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TransactionDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("transactionDescription")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TransactionId)
                    .IsUnicode(false)
                    .HasColumnName("transactionId");

                entity.Property(e => e.Usrdata)
                    .HasColumnType("datetime")
                    .HasColumnName("usrdata")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Usrhora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("usrhora")
                    .HasDefaultValueSql("(left(CONVERT([time],getdate()),(8)))");

                entity.Property(e => e.Usrinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("usrinis")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Valor)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("valor");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
