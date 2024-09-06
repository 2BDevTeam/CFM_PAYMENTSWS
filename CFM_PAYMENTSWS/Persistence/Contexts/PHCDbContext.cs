using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.Extensions;

namespace CFM_PAYMENTSWS.Persistence.Contexts
{
    public partial class PHCDbContext : DbContext
    {
        public PHCDbContext()
        {
        }

        public PHCDbContext(DbContextOptions<PHCDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<JobLocks> JobLocks { get; set; } = null!;
        public virtual DbSet<Po> Po { get; set; } = null!;
        public virtual DbSet<Liame> Liame { get; set; } = null!;
        public virtual DbSet<UWspayments> UWspayments { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ConnStrE14"));
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

            /*
            if (Database.IsSqlServer())
            {
                modelBuilder.AddSqlConvertFunction();
            }
            */
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<UWspayments>(entity =>
            {
                entity.HasKey(e => e.UWspaymentsstamp)
                    .HasName("pk_u_wspayments")
                    .IsClustered(false);

                entity.ToTable("u_wspayments");

                entity.Property(e => e.UWspaymentsstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_wspaymentsstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Bankreference)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bankreference")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Batchid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("batchid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Dataprocessado)
                    .HasColumnType("datetime")
                    .HasColumnName("dataprocessado")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

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

                entity.Property(e => e.Estado)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Origem)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("origem")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Oristamp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("oristamp")
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

                entity.Property(e => e.Userno)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userno")
                    .HasDefaultValueSql("('')");
            });


            modelBuilder.Entity<Po>(entity =>
            {
                entity.HasKey(e => new { e.Rno, e.Poano })
                    .HasName("pk_po")
                    .IsClustered(false);
                entity.ToTable("po");
                entity.HasIndex(e => e.No, "in_po_no")
                    .HasFillFactor(80);
                entity.HasIndex(e => e.Nome, "in_po_nome")
                    .HasFillFactor(80);
                entity.HasIndex(e => new { e.Rno, e.Rdata, e.Nome, e.Total, e.Etotal, e.Postamp }, "in_po_polist")
                    .HasFillFactor(80);
                entity.HasIndex(e => e.Rdata, "in_po_rdata")
                    .HasFillFactor(80);
                entity.HasIndex(e => e.Postamp, "in_po_stamp")
                    .IsUnique()
                    .HasFillFactor(80);
                entity.Property(e => e.Rno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("rno");
                entity.Property(e => e.Poano)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("poano");
                entity.Property(e => e.Acerto)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("acerto");
                entity.Property(e => e.Adoc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("adoc")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Arred)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("arred");
                entity.Property(e => e.Bic)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("bic")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Cativa)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("cativa");
                entity.Property(e => e.Cc2stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cc2stamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();
                entity.Property(e => e.Ccusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ccusto")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Chftotal)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("chftotal");
                entity.Property(e => e.Chtmoeda)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("chtmoeda");
                entity.Property(e => e.Chtotal)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("chtotal");
                entity.Property(e => e.Cm)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("cm");
                entity.Property(e => e.Cmdesc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cmdesc")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Cobranca)
                    .HasMaxLength(22)
                    .IsUnicode(false)
                    .HasColumnName("cobranca")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Codpost)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("codpost")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Contado)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("contado");
                entity.Property(e => e.Contado2)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("contado2");
                entity.Property(e => e.Cxstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cxstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();
                entity.Property(e => e.Cxusername)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cxusername")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Desc1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("desc1")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Desc2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("desc2")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Descba)
                    .HasMaxLength(33)
                    .IsUnicode(false)
                    .HasColumnName("descba")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Diaplano)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("diaplano");
                entity.Property(e => e.Difcambio)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("difcambio");
                entity.Property(e => e.Dilnoplano)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("dilnoplano");
                entity.Property(e => e.Dinoplano)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("dinoplano")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Dostamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("dostamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();
                entity.Property(e => e.Dplano)
                    .HasColumnType("datetime")
                    .HasColumnName("dplano")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");
                entity.Property(e => e.Dvalor)
                    .HasColumnType("datetime")
                    .HasColumnName("dvalor")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");
                entity.Property(e => e.Eacerto)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eacerto");
                entity.Property(e => e.Earred)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("earred");
                entity.Property(e => e.Earredm2)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("earredm2");
                entity.Property(e => e.Ecativa)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ecativa");
                entity.Property(e => e.Echftotal)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("echftotal");
                entity.Property(e => e.Echtotal)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("echtotal");
                entity.Property(e => e.Edifcambio)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("edifcambio");
                entity.Property(e => e.Efinv)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("efinv");
                entity.Property(e => e.Eivav1)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivav1");
                entity.Property(e => e.Eivav2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivav2");
                entity.Property(e => e.Eivav3)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivav3");
                entity.Property(e => e.Eivav4)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivav4");
                entity.Property(e => e.Eivav5)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivav5");
                entity.Property(e => e.Eivav6)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivav6");
                entity.Property(e => e.Eivav7)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivav7");
                entity.Property(e => e.Eivav8)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivav8");
                entity.Property(e => e.Eivav9)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivav9");
                entity.Property(e => e.Epaga1)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("epaga1");
                entity.Property(e => e.Epaga2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("epaga2");
                entity.Property(e => e.Epaga3)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("epaga3");
                entity.Property(e => e.Epaga4)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("epaga4");
                entity.Property(e => e.Epaga5)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("epaga5");
                entity.Property(e => e.Epaga6)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("epaga6");
                entity.Property(e => e.Estab)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("estab");
                entity.Property(e => e.Etotal)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("etotal");
                entity.Property(e => e.Etotol2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("etotol2");
                entity.Property(e => e.Etotow)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("etotow");
                entity.Property(e => e.Evdinheiro)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evdinheiro");
                entity.Property(e => e.Evirs)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evirs");
                entity.Property(e => e.Exportado).HasColumnName("exportado");
                entity.Property(e => e.Faztrf).HasColumnName("faztrf");
                entity.Property(e => e.Fcstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("fcstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();
                entity.Property(e => e.Fin)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("fin");
                entity.Property(e => e.Finv)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("finv");
                entity.Property(e => e.Finvmoeda)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("finvmoeda");
                entity.Property(e => e.Fref)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("fref")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Iban)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("iban")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Impresso).HasColumnName("impresso");
                entity.Property(e => e.Intid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("intid")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Introfin).HasColumnName("introfin");
                entity.Property(e => e.Ivacaixa).HasColumnName("ivacaixa");
                entity.Property(e => e.Ivav1)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivav1");
                entity.Property(e => e.Ivav2)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivav2");
                entity.Property(e => e.Ivav3)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivav3");
                entity.Property(e => e.Ivav4)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivav4");
                entity.Property(e => e.Ivav5)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivav5");
                entity.Property(e => e.Ivav6)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivav6");
                entity.Property(e => e.Ivav7)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivav7");
                entity.Property(e => e.Ivav8)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivav8");
                entity.Property(e => e.Ivav9)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivav9");
                entity.Property(e => e.Jatrfonline).HasColumnName("jatrfonline");
                entity.Property(e => e.Local)
                    .HasMaxLength(43)
                    .IsUnicode(false)
                    .HasColumnName("local")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Luserfin).HasColumnName("luserfin");
                entity.Property(e => e.Macerto)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("macerto");
                entity.Property(e => e.Marcada).HasColumnName("marcada");
                entity.Property(e => e.Mcativa)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("mcativa");
                entity.Property(e => e.Memissao)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("memissao")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Modop1)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("modop1")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Modop2)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("modop2")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Modop3)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("modop3")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Modop4)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("modop4")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Modop5)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("modop5")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Modop6)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("modop6")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Moeda)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("moeda")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Moeda2)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("moeda2")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Moeda3)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("moeda3")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Morada)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("morada")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Mpaga1)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("mpaga1");
                entity.Property(e => e.Mpaga2)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("mpaga2");
                entity.Property(e => e.Mpaga3)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("mpaga3");
                entity.Property(e => e.Mpaga4)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("mpaga4");
                entity.Property(e => e.Mpaga5)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("mpaga5");
                entity.Property(e => e.Mpaga6)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("mpaga6");
                entity.Property(e => e.Mvdinheiro)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("mvdinheiro");
                entity.Property(e => e.Ncont)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ncont")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Ncusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ncusto")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Nib)
                    .HasMaxLength(28)
                    .IsUnicode(false)
                    .HasColumnName("nib")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.No)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("no");
                entity.Property(e => e.Nome)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("nome")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();
                entity.Property(e => e.Nome2)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("nome2")
                    .HasDefaultValueSql("('')");
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
                entity.Property(e => e.Ollocal2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ollocal2")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Olstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("olstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();
                entity.Property(e => e.Operext).HasColumnName("operext");
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
                entity.Property(e => e.Paga1)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("paga1");
                entity.Property(e => e.Paga2)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("paga2");
                entity.Property(e => e.Paga3)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("paga3");
                entity.Property(e => e.Paga4)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("paga4");
                entity.Property(e => e.Paga5)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("paga5");
                entity.Property(e => e.Paga6)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("paga6");
                entity.Property(e => e.Pais)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("pais");
                entity.Property(e => e.Plano).HasColumnName("plano");
                entity.Property(e => e.Planoonline).HasColumnName("planoonline");
                entity.Property(e => e.Pno)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("pno");
                entity.Property(e => e.Pnome)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("pnome")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Poid)
                    .HasColumnType("numeric(12, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("poid");
                entity.Property(e => e.Postamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("postamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();
                entity.Property(e => e.Procdata)
                    .HasColumnType("datetime")
                    .HasColumnName("procdata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");
                entity.Property(e => e.Process).HasColumnName("process");
                entity.Property(e => e.Processsepa).HasColumnName("processsepa");
                entity.Property(e => e.Rdata)
                    .HasColumnType("datetime")
                    .HasColumnName("rdata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");
                entity.Property(e => e.Regiva).HasColumnName("regiva");
                entity.Property(e => e.Segmento)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("segmento")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Sepagh)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("sepagh")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Sepapi)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("sepapi")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Site)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("site")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Ssstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ssstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();
                entity.Property(e => e.Ssusername)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ssusername")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Tbcheque)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("tbcheque")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Tbok).HasColumnName("tbok");
                entity.Property(e => e.Tbstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("tbstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();
                entity.Property(e => e.Telocal)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("telocal")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Telocal2)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("telocal2")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Tipo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tipo")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Total)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("total");
                entity.Property(e => e.Totalmoeda)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("totalmoeda");
                entity.Property(e => e.Totol2)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("totol2");
                entity.Property(e => e.Totow)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("totow");
                entity.Property(e => e.Txirs)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("txirs");
                entity.Property(e => e.UBalcao)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("u_balcao")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.UBanco)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("u_banco")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.UBenef)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_benef")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.UConta)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_conta")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.UDatatrf)
                    .HasColumnType("datetime")
                    .HasColumnName("u_datatrf")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");
                entity.Property(e => e.UDesc)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("u_desc")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.UFicheiro)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_ficheiro")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.UFollocal)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("u_follocal")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.UImpresso).HasColumnName("u_impresso");
                entity.Property(e => e.UIncluido).HasColumnName("u_incluido");
                entity.Property(e => e.UIntgrok).HasColumnName("u_intgrok");
                entity.Property(e => e.UMcam)
                    .HasColumnType("numeric(16, 3)")
                    .HasColumnName("u_mcam");
                entity.Property(e => e.UMotivtrf)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("u_motivtrf")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.UNib)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_nib")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.UNome)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("u_nome")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.UNref)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("u_nref")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.UOwirps).HasColumnName("u_owirps");
                entity.Property(e => e.UPocam)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_pocam");
                entity.Property(e => e.UPotot)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_potot");
                entity.Property(e => e.URefbanco)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_refbanco")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.USendpay).HasColumnName("u_sendpay");
                entity.Property(e => e.UUsrtrf)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("u_usrtrf")
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.UVerif).HasColumnName("u_verif");
                entity.Property(e => e.UVirsm)
                    .HasColumnType("numeric(16, 0)")
                    .HasColumnName("u_virsm");
                entity.Property(e => e.Userimpresso)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("userimpresso")
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
                entity.Property(e => e.Valor2m2)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("valor2m2");
                entity.Property(e => e.Valor2m3)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("valor2m3");
                entity.Property(e => e.Valorm2)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("valorm2");
                entity.Property(e => e.Valorowm2)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("valorowm2");
                entity.Property(e => e.Vdinheiro)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("vdinheiro");
                entity.Property(e => e.Virs)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("virs");
                entity.Property(e => e.Zona)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("zona")
                    .HasDefaultValueSql("('')");
            });


            modelBuilder.Entity<JobLocks>(entity =>
            {
                entity.HasKey(e => e.JobId)
                    .HasName("PK__JobLocks__056690C2F4BFF789");
                entity.Property(e => e.JobId).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}


//AppDbContext