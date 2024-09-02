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

        public virtual DbSet<U2bPaymentsHsTs> U2bPaymentsHsTs { get; set; } = null!;

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
            modelBuilder.Entity<U2bPaymentsHsTs>(entity =>
            {
                entity.HasKey(e => e.U2bPaymentsHsTsstamp)
                    .HasName("PK_U_2b_Payments_Hs_ts");

                entity.ToTable("u_2b_payments_hs_ts");

                entity.Property(e => e.U2bPaymentsHsTsstamp)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_2b_payments_hs_tsstamp")
                    .HasDefaultValueSql("(left(newid(),(25)))");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BankReference)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BatchId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BeneficiaryName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CreditAccount)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DebitAccount)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("debitAccount")
                    .HasDefaultValueSql("(left(newid(),(25)))");

                entity.Property(e => e.Oristamp)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("oristamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ousrdata)
                    .HasColumnType("datetime")
                    .HasColumnName("ousrdata")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ProcessingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("processingDate")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StatusCodeHs)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("statusCodeHs")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StatusDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StatusDescriptionHs)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("statusDescriptionHs")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TransactionDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
