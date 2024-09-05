using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.Extensions;

namespace CFM_PAYMENTSWS.Persistence.Contexts
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApiLogs> ApiLogs { get; set; } = null!;
        public virtual DbSet<Log> Log { get; set; } = null!;
        public virtual DbSet<U2BPayments> U2BPayments { get; set; } = null!;
        public virtual DbSet<U2BPaymentsQueue> U2BPaymentsQueue { get; set; } = null!;
        public virtual DbSet<U2BPaymentsHs> U2BPaymentHs { get; set; } = null!;
        public virtual DbSet<E4> E4 { get; set; } = null!;
        public virtual DbSet<UProvider> UProvider { get; set; } = null!;
        public virtual DbSet<ApiKey> ApiKey { get; set; } = null!;
        public virtual DbSet<Key> Key { get; set; } = null!;

        /*
         * Testes
        */
        public virtual DbSet<U2bPaymentsHsTs> U2bPaymentsHsTs { get; set; }
        public virtual DbSet<U2bPaymentsTs> U2bPaymentsTs { get; set; }
        public virtual DbSet<U2bPaymentsQueueTs> U2bPaymentsQueueTs { get; set; }
        public virtual DbSet<Suliame> Suliame { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ConnStr"));
        }

        public string GetModelNameForTable(string tableName)
        {
            var entityType = Model.GetEntityTypes()
             .FirstOrDefault(et => string.Equals(et.GetTableName(), tableName, StringComparison.OrdinalIgnoreCase));

            return entityType?.ClrType.Name ?? "Modelo não encontrado";
        }

        public string GetPropertyNameForColumn(string tableName, string columnName)
        {
            var entityType = Model.GetEntityTypes()
                .FirstOrDefault(et => string.Equals(et.GetTableName(), tableName, StringComparison.OrdinalIgnoreCase));

            if (entityType == null)
                return "Tabela não encontrada";

            foreach (var property in entityType.GetProperties())
            {
                if (string.Equals(property.GetColumnName(), columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return property.Name;
                }
            }

            return "Coluna não encontrada";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (Database.IsSqlServer())
            {
                modelBuilder.AddSqlConvertFunction();
            }
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

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


            modelBuilder.Entity<ApiLogs>(entity =>
            {
                entity.HasKey(e => e.UApilogstamp)
                    .HasName("PK__api_logs__D377A6C4446863E2");

                entity.ToTable("u_api_logs");

                entity.Property(e => e.UApilogstamp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_apilogstamp");

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Content)
                    .IsUnicode(false)
                    .HasColumnName("content");

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("data");

                entity.Property(e => e.Operation)
                    .IsUnicode(false)
                    .HasColumnName("operation");

                entity.Property(e => e.RequestId)
                    .IsUnicode(false)
                    .HasColumnName("requestId");

                entity.Property(e => e.ResponseDesc)
                    .IsUnicode(false)
                    .HasColumnName("responseDesc");

                entity.Property(e => e.ResponseText)
                    .IsUnicode(false)
                    .HasColumnName("responsetext");
            });


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
                    .HasDefaultValueSql("('')")
                    .HasColumnName("batchID");
                entity.Property(e => e.beneficiaryName)
                    .HasColumnType("text")
                    .HasColumnName("beneficiaryName");
                entity.Property(e => e.canal).HasColumnName("canal");
                entity.Property(e => e.descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("descricao");
                entity.Property(e => e.description)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("description");
                entity.Property(e => e.destino)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("destino");
                entity.Property(e => e.docno)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("docno");
                entity.Property(e => e.BeneficiaryEmail)
                    .HasColumnType("text")
                    .HasColumnName("emailf");
                entity.Property(e => e.estado)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("estado");
                entity.Property(e => e.keystamp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("keystamp");
                entity.Property(e => e.lordem)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("lordem");
                entity.Property(e => e.Marcada).HasColumnName("marcada");
                entity.Property(e => e.moeda)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("moeda");
                entity.Property(e => e.origem)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("origem");
                entity.Property(e => e.Oristamp)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("oristamp");
                entity.Property(e => e.Ousrdata)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("date")
                    .HasColumnName("ousrdata");
                entity.Property(e => e.Ousrhora)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(left(CONVERT([time],getdate()),(8)))")
                    .HasColumnName("ousrhora");
                entity.Property(e => e.Ousrinis)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ousrinis");
                entity.Property(e => e.processingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("processingDate");
                entity.Property(e => e.transactionDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("transactionDescription");
                entity.Property(e => e.transactionId)
                    .IsUnicode(false)
                    .HasColumnName("transactionId");
                entity.Property(e => e.usrdata)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("usrdata");
                entity.Property(e => e.Usrhora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(left(CONVERT([time],getdate()),(8)))")
                    .HasColumnName("usrhora");
                entity.Property(e => e.Usrinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("usrinis");
                entity.Property(e => e.valor)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("valor");
            });


            modelBuilder.Entity<U2bPaymentsTs>(entity =>
            {
                entity.HasKey(e => e.U2bPaymentsTsstamp).HasName("PK__u_2b_Pay__815B88F3550AE875");

                entity.ToTable("u_2b_Payments_ts");

                entity.Property(e => e.U2bPaymentsTsstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_2b_Payments_tsstamp");
                entity.Property(e => e.bankReference)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("bankReference");
                entity.Property(e => e.BatchId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Canal)
                    .HasDefaultValueSql("((0))")
                    .HasColumnName("canal");
                entity.Property(e => e.Checked).HasColumnName("checked");
                entity.Property(e => e.dataprocessado)
                    .HasDefaultValueSql("('1900-01-01')")
                    .HasColumnType("datetime")
                    .HasColumnName("dataprocessado");
                entity.Property(e => e.descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("descricao");
                entity.Property(e => e.Destino)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("destino");
                entity.Property(e => e.Docno)
                    .IsUnicode(false)
                    .HasColumnName("docno");
                entity.Property(e => e.Emailf)
                    .HasDefaultValueSql("('')")
                    .HasColumnType("text")
                    .HasColumnName("emailf");
                entity.Property(e => e.estado)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("estado");
                entity.Property(e => e.Horaprocessado)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("horaprocessado");
                entity.Property(e => e.Keystamp)
                    .IsUnicode(false)
                    .HasColumnName("keystamp");
                entity.Property(e => e.Lancadonatesouraria)
                    .HasDefaultValueSql("((0))")
                    .HasColumnName("lancadonatesouraria");
                entity.Property(e => e.Lordem)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("lordem");
                entity.Property(e => e.Marcada).HasColumnName("marcada");
                entity.Property(e => e.Moeda)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("moeda");
                entity.Property(e => e.MpesaTransactionid)
                    .IsUnicode(false)
                    .HasColumnName("mpesa_transactionid");
                entity.Property(e => e.Origem)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("origem");
                entity.Property(e => e.Oristamp)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("oristamp");
                entity.Property(e => e.Ousrdata)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("date")
                    .HasColumnName("ousrdata");
                entity.Property(e => e.Ousrhora)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(left(CONVERT([time],getdate()),(8)))")
                    .HasColumnName("ousrhora");
                entity.Property(e => e.Ousrinis)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ousrinis");
                entity.Property(e => e.ProcessingDateHs)
                    .HasDefaultValueSql("('19000101')")
                    .HasColumnType("datetime")
                    .HasColumnName("processingDateHs");
                entity.Property(e => e.StatusCodeHs)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("statusCodeHs");
                entity.Property(e => e.StatusDescriptionHs)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("statusDescriptionHs");
                entity.Property(e => e.Tabela)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("tabela");
                entity.Property(e => e.transactionId)
                    .IsUnicode(false)
                    .HasColumnName("transactionid");
                entity.Property(e => e.usrdata)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("usrdata");
                entity.Property(e => e.Usrhora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(left(CONVERT([time],getdate()),(8)))")
                    .HasColumnName("usrhora");
                entity.Property(e => e.Usrinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("usrinis");
                entity.Property(e => e.Valor)
                    .HasColumnType("decimal(16, 2)")
                    .HasColumnName("valor");
            });


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


            modelBuilder.Entity<Log>(entity =>
            {

                entity.HasKey(e => e.u_logstamp);
                entity.ToTable("u_logs");
                entity.Property(e => e.u_logstamp).HasColumnName("u_logsstamp");
                entity.Property(e => e.RequestId);
                entity.Property(e => e.Data);
                entity.Property(e => e.Code);
                entity.Property(e => e.Content);
                entity.Property(e => e.ResponseDesc);
                entity.Property(e => e.Operation);

            });
            modelBuilder.Entity<UProvider>(entity =>
            {

                entity.HasKey(e => e.u_providerstamp);
                entity.ToTable("u_provider");
                entity.Property(e => e.u_providerstamp).HasColumnName("u_providerstamp");
                entity.Property(e => e.chave);
                entity.Property(e => e.grupo);
                entity.Property(e => e.codigo);
                entity.Property(e => e.valor);


            });

            modelBuilder.Entity<E4>(entity =>
            {

                entity.HasKey(e => e.e4stamp);
                entity.ToTable("e4");
                entity.Property(e => e.e4stamp).HasColumnName("e4stamp");
                entity.Property(e => e.u_pubkey);
                entity.Property(e => e.u_apikey);


            });

            modelBuilder.Entity<Key>(entity =>
            {
                entity.HasKey(e => e.keystamp);
                entity.ToTable("basdsf");
                entity.Property(e => e.keystamp).HasColumnName("basdsfstamp");
                entity.Property(e => e.key).HasColumnName("basdsf1");
                entity.Property(e => e.isActive).HasColumnName("basdsf2");

            });

            modelBuilder.Entity<U2BPayments>(entity =>
            {

                entity.HasKey(e => e.u_2b_paymentsstamp);

                /*
                entity.ToTable("u_2b_payments", e =>
                {
                    e.HasTrigger("u_2b_trg_update_po_on_success");
                });
                */
                entity.Property(e => e.u_2b_paymentsstamp).HasColumnName("u_2b_paymentsstamp");
                entity.Property(e => e.valor);
                entity.Property(e => e.destino);
                entity.Property(e => e.origem);
                entity.Property(e => e.tabela);
                entity.Property(e => e.moeda);
                entity.Property(e => e.estado);
                entity.Property(e => e.descricao);
                entity.Property(e => e.lordem);
                entity.Property(e => e.ousrinis);
                entity.Property(e => e.ousrdata);
                entity.Property(e => e.ousrhora);
                entity.Property(e => e.usrinis);
                entity.Property(e => e.usrdata);
                entity.Property(e => e.usrhora);
                entity.Property(e => e.oristamp);
                entity.Property(e => e.Processado);
                entity.Property(e => e.dataprocessado);
                entity.Property(e => e.horaprocessado);
                entity.Property(e => e.canal);
                entity.Property(e => e.transactionId);
                entity.Property(e => e.BatchId);
                entity.Property(e => e.bankReference);
                entity.Property(e => e.BeneficiaryEmail).HasColumnName("emailf");


            });
            modelBuilder.Entity<U2BPaymentsQueue>(entity =>
            {

                entity.HasKey(e => e.u_2b_paymentsQueuestamp);
                entity.ToTable("u_2b_paymentsQueue");
                entity.Property(e => e.u_2b_paymentsQueuestamp).HasColumnName("u_2b_paymentsQueuestamp");
                entity.Property(e => e.valor);
                entity.Property(e => e.origem);
                entity.Property(e => e.BatchId);
                entity.Property(e => e.description);
                entity.Property(e => e.beneficiaryName);
                entity.Property(e => e.transactionDescription);
                entity.Property(e => e.destino);
                entity.Property(e => e.lordem);
                entity.Property(e => e.canal);
                entity.Property(e => e.transactionId);
                entity.Property(e => e.estado);
                entity.Property(e => e.keystamp);
                entity.Property(e => e.processingDate);
                entity.Property(e => e.BeneficiaryEmail).HasColumnName("emailf"); ;


            });

            modelBuilder.Entity<U2BPaymentsHs>(entity =>
            {
                entity.HasKey(e => e.U2BPaymentsHsStamp);
                entity.ToTable("u_2b_payments_hs");
                entity.Property(e => e.U2BPaymentsHsStamp).HasColumnName("u_2b_payments_hsstamp");
                entity.Property(e => e.Amount);
                entity.Property(e => e.BankReference);
                entity.Property(e => e.BatchId);
                entity.Property(e => e.BeneficiaryName);
                entity.Property(e => e.CreditAccount);
                entity.Property(e => e.Currency);
                entity.Property(e => e.DebitAccount);
                entity.Property(e => e.OusrData);
                entity.Property(e => e.ProcessingDate);
                entity.Property(e => e.StatusCode);
                entity.Property(e => e.StatusDescription);
                entity.Property(e => e.StatusCodeHs);
                entity.Property(e => e.StatusDescriptionHs);
                entity.Property(e => e.TransactionDescription);
                entity.Property(e => e.TransactionId);

            });

            modelBuilder.Entity<ApiKey>(entity =>
            {

                entity.HasKey(e => e.u_apikeystamp);
                entity.ToTable("u_apikey");
                entity.Property(e => e.u_apikeystamp).HasColumnName("u_apikeystamp");
                entity.Property(e => e.apikey);
                entity.Property(e => e.entity);
                entity.Property(e => e.createdAt);


            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}


//AppDbContext