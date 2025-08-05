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
        public virtual DbSet<U2bPayments> U2bPayments { get; set; } = null!;
        public virtual DbSet<U2bPaymentsQueue> U2bPaymentsQueue { get; set; } = null!;
        public virtual DbSet<U2bPaymentsHs> U2bPaymentsHs { get; set; } = null!;
      
        public virtual DbSet<E4> E4 { get; set; } = null!;
        public virtual DbSet<UProvider> UProvider { get; set; } = null!;
        public virtual DbSet<ApiKey> ApiKey { get; set; } = null!;
        public virtual DbSet<Key> Key { get; set; } = null!;

        /*
         * Testes
        */
        /*
        public virtual DbSet<U2bPaymentsHs> U2bPaymentsHs { get; set; }
        public virtual DbSet<U2bPayments> U2bPayments { get; set; }
        public virtual DbSet<U2bPaymentsQueue> U2bPaymentsQueue { get; set; }
        */
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

            modelBuilder.Entity<U2bPayments>(entity =>
            {
                entity.HasKey(e => e.U2bPaymentsstamp)
                    .HasName("PK__u_2b_Pay__95E0972B6D99F5AF");

                entity.ToTable("u_2b_Payments");

                entity.Property(e => e.U2bPaymentsstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_2b_paymentsstamp");

                entity.Property(e => e.BankReference)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bankReference")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BatchId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Canal)
                    .HasColumnName("canal")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Checked).HasColumnName("checked");

                entity.Property(e => e.Dataprocessado)
                    .HasColumnType("datetime")
                    .HasColumnName("dataprocessado")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("descricao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Destino)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("destino")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Docno)
                    .IsUnicode(false)
                    .HasColumnName("docno");

                entity.Property(e => e.Emailf)
                    .HasColumnType("text")
                    .HasColumnName("emailf")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Estado)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Horaprocessado)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("horaprocessado")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Keystamp)
                    .IsUnicode(false)
                    .HasColumnName("keystamp");

                entity.Property(e => e.Lancadonatesouraria)
                    .HasColumnName("lancadonatesouraria")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Lordem)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("lordem");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Moeda)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("moeda")
                    .HasDefaultValueSql("('')");

                //entity.Property(e => e.Tipo)
                //    .IsUnicode(false)
                //    .HasColumnName("tipo");

                entity.Property(e => e.Origem)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("origem")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Oristamp)
                    .IsUnicode(false)
                    .HasColumnName("oristamp")
                    .HasDefaultValueSql("('')");

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

                entity.Property(e => e.ProcessingDateHs)
                    .HasColumnType("datetime")
                    .HasColumnName("processingDateHs")
                    .HasDefaultValueSql("('19000101')");

                entity.Property(e => e.StatusCodeHs)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("statusCodeHs")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StatusDescriptionHs)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("statusDescriptionHs")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tabela)
                    .IsUnicode(false)
                    .HasColumnName("tabela")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Transactionid)
                    .IsUnicode(false)
                    .HasColumnName("transactionid");

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

            modelBuilder.Entity<U2bPaymentsQueue>(entity =>
            {
                entity.HasKey(e => e.U2bPaymentsQueuestamp)
                    .HasName("pk_u_2b_paymentsQueue")
                    .IsClustered(false);

                entity.ToTable("u_2b_paymentsQueue");

                entity.Property(e => e.U2bPaymentsQueuestamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_2b_paymentsQueuestamp");

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
                    .HasColumnName("emailf")
                    .HasDefaultValueSql("('')");

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
                    .HasColumnName("processingDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Tabela)
                    .IsUnicode(false)
                    .HasColumnName("tabela")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ccusto)
                    .IsUnicode(false)
                    .HasColumnName("ccusto")
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

            modelBuilder.Entity<U2bPaymentsHs>(entity =>
            {
                entity.HasKey(e => e.U2bPaymentsHsstamp);

                entity.ToTable("u_2b_payments_hs");

                entity.Property(e => e.U2bPaymentsHsstamp)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_2b_payments_hsstamp")
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