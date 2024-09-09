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
        public virtual DbSet<Ol> Ol { get; set; } = null!;
        public virtual DbSet<Pd> Pd { get; set; } = null!;
        public virtual DbSet<UTrfb> UTrfb { get; set; } = null!;


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


            modelBuilder.Entity<Ol>(entity =>
            {
                entity.HasKey(e => e.Olstamp)
                    .HasName("pk_ol")
                    .IsClustered(false);

                entity.ToTable("ol");

                entity.HasIndex(e => e.Chlstamp, "in_ol_chlstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Cxstamp, "in_ol_cxstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Data, "in_ol_data")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Facastamp, "in_ol_facastamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Faccstamp, "in_ol_faccstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Facdstamp, "in_ol_facdstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Fcxstamp, "in_ol_fcxstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Fostamp, "in_ol_fostamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Frstamp, "in_ol_frstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Ftstamp, "in_ol_ftstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Gpcstamp, "in_ol_gpcstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Lmstamp, "in_ol_lmstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Lostamp, "in_ol_lostamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Lpstamp, "in_ol_lpstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Lrstamp, "in_ol_lrstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Mlstamp, "in_ol_mlstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Olbbstamp, "in_ol_olbbstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Olbcstamp, "in_ol_olbcstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Data, "in_ol_ollist")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Oristamp, "in_ol_oristamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Owlistamp, "in_ol_owlistamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Owstamp, "in_ol_owstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Oxstamp, "in_ol_oxstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Ozstamp, "in_ol_ozstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Pdstamp, "in_ol_pdstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Postamp, "in_ol_postamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Prstamp, "in_ol_prstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Rdstamp, "in_ol_rdstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Restamp, "in_ol_restamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Ssstamp, "in_ol_ssstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Tbstamp, "in_ol_tbstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Vrstamp, "in_ol_vrstamp")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Vzstamp, "in_ol_vzstamp")
                    .HasFillFactor(80);

                entity.Property(e => e.Olstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("olstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Acerto)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("acerto");

                entity.Property(e => e.Ccusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ccusto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cecope)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("cecope")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cheque)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cheque")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Chlstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("chlstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Chtmoeda)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("chtmoeda");

                entity.Property(e => e.Chtotal)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("chtotal");

                entity.Property(e => e.Contado)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("contado");

                entity.Property(e => e.Cxstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cxstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cxusername)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cxusername")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("data")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(85)
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
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("documento")
                    .HasDefaultValueSql("('')");

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

                entity.Property(e => e.Echtotal)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("echtotal");

                entity.Property(e => e.Eentr)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eentr");

                entity.Property(e => e.Entr)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("entr");

                entity.Property(e => e.Entrm)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("entrm");

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

                entity.Property(e => e.Esaid)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("esaid");

                entity.Property(e => e.Evdinheiro)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evdinheiro");

                entity.Property(e => e.Exportado).HasColumnName("exportado");

                entity.Property(e => e.Facastamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("facastamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Faccstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("faccstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Facdstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("facdstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Fcxstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("fcxstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Fostamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("fostamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fref)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("fref")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Frstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("frstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Ftstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ftstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gpcstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("gpcstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Grupo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("grupo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Intid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("intid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Lmstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("lmstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Local)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("local")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Lostamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("lostamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Lpstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("lpstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Lrstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("lrstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Macerto)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("macerto");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Mlstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("mlstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

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

                entity.Property(e => e.Ncusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ncusto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Olbbstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("olbbstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Olbcstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("olbcstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Olcodigo)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("olcodigo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Olid)
                    .HasColumnType("numeric(12, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("olid");

                entity.Property(e => e.Ollocal)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ollocal")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Operext).HasColumnName("operext");

                entity.Property(e => e.Origem)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("origem")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Oristamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("oristamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

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

                entity.Property(e => e.Owlistamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("owlistamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Owstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("owstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Oxstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("oxstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ozstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ozstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

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

                entity.Property(e => e.Pdstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("pdstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Plano).HasColumnName("plano");

                entity.Property(e => e.Pno)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("pno");

                entity.Property(e => e.Pnome)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("pnome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Postamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("postamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Prstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("prstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Rdstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("rdstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Restamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("restamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Said)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("said");

                entity.Property(e => e.Saidm)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("saidm");

                entity.Property(e => e.Sgrupo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("sgrupo")
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
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ssusername)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ssusername")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tbstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("tbstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Transf).HasColumnName("transf");

                entity.Property(e => e.UBostamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_bostamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDataobra)
                    .HasColumnType("datetime")
                    .HasColumnName("u_dataobra")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.UDesc)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("u_desc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UEnt)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_ent");

                entity.Property(e => e.UInclpr).HasColumnName("u_inclpr");

                entity.Property(e => e.UMcam)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_mcam");

                entity.Property(e => e.UMoe)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("u_moe")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UObrano)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("u_obrano");

                entity.Property(e => e.UOlcam)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_olcam");

                entity.Property(e => e.UOltot)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_oltot");

                entity.Property(e => e.USaid)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_said");

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

                entity.Property(e => e.Vdinheiro)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("vdinheiro");

                entity.Property(e => e.Vrstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("vrstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Vzstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("vzstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Pd>(entity =>
            {
                entity.HasKey(e => new { e.Rno, e.Pdano })
                    .HasName("pk_pd")
                    .IsClustered(false);

                entity.ToTable("pd");

                entity.HasIndex(e => e.Nome, "in_pd_nome")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.Rno, e.Rdata, e.Nome, e.Total, e.Pdstamp }, "in_pd_pdlist")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Pdstamp, "in_pd_stamp")
                    .IsUnique()
                    .HasFillFactor(80);

                entity.Property(e => e.Rno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("rno");

                entity.Property(e => e.Pdano)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("pdano");

                entity.Property(e => e.Adoc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("adoc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Base)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("base");

                entity.Property(e => e.Basemoeda)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("basemoeda");

                entity.Property(e => e.Bempr).HasColumnName("bempr");

                entity.Property(e => e.Cambio)
                    .HasColumnType("numeric(20, 12)")
                    .HasColumnName("cambio");

                entity.Property(e => e.Cambiofixo).HasColumnName("cambiofixo");

                entity.Property(e => e.Ccusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ccusto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cecope)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("cecope")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cheque)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("cheque")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Chfollocal)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("chfollocal")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Chfstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("chfstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Chfvalor)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("chfvalor");

                entity.Property(e => e.Cm)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("cm");

                entity.Property(e => e.Cmdesc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cmdesc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Codpost)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("codpost")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Contado)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("contado");

                entity.Property(e => e.Crend)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("crend")
                    .HasDefaultValueSql("('')");

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

                entity.Property(e => e.Czonag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("czonag")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Dataven)
                    .HasColumnType("datetime")
                    .HasColumnName("dataven")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
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

                entity.Property(e => e.Ebase)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ebase");

                entity.Property(e => e.Echfvalor)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("echfvalor");

                entity.Property(e => e.Eivav)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivav");

                entity.Property(e => e.Estab)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("estab");

                entity.Property(e => e.Etotal)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("etotal");

                entity.Property(e => e.Evirs)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evirs");

                entity.Property(e => e.Exportado).HasColumnName("exportado");

                entity.Property(e => e.Formapag)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("formapag")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fref)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("fref")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Identdecexp)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("identdecexp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Incerto).HasColumnName("incerto");

                entity.Property(e => e.Integrado).HasColumnName("integrado");

                entity.Property(e => e.Intid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("intid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Iva)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("iva");

                entity.Property(e => e.Ivav)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivav");

                entity.Property(e => e.Ivavmoeda)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("ivavmoeda");

                entity.Property(e => e.Jatrfonline).HasColumnName("jatrfonline");

                entity.Property(e => e.Local)
                    .HasMaxLength(43)
                    .IsUnicode(false)
                    .HasColumnName("local")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Memissao)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("memissao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Modalidade)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("modalidade")
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

                entity.Property(e => e.Morada)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("morada")
                    .HasDefaultValueSql("('')");

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

                entity.Property(e => e.Pais)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("pais");

                entity.Property(e => e.Pdid)
                    .HasColumnType("numeric(12, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("pdid");

                entity.Property(e => e.Pdstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("pdstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

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

                entity.Property(e => e.Processo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("processo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Rdata)
                    .HasColumnType("datetime")
                    .HasColumnName("rdata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Segmento)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("segmento")
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

                entity.Property(e => e.Subproc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("subproc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Sujirs).HasColumnName("sujirs");

                entity.Property(e => e.Sujirsisen).HasColumnName("sujirsisen");

                entity.Property(e => e.Tabiva)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("tabiva");

                entity.Property(e => e.Telocal)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("telocal")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tipo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tipoad)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("tipoad");

                entity.Property(e => e.Total)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("total");

                entity.Property(e => e.Totalmoeda)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("totalmoeda");

                entity.Property(e => e.Txirs)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("txirs");

                entity.Property(e => e.UAnulado).HasColumnName("u_anulado");

                entity.Property(e => e.UAtt)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_att")
                    .HasDefaultValueSql("('')");

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

                entity.Property(e => e.UBopstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_bopstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UBostamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_bostamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UCheqentr).HasColumnName("u_cheqentr");

                entity.Property(e => e.UCm)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("u_cm");

                entity.Property(e => e.UColaborr)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_colaborr")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UConta)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_conta")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UContacto)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("u_contacto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDatatrf)
                    .HasColumnType("datetime")
                    .HasColumnName("u_datatrf")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.UDesc)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("u_desc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDevoluc)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_devoluc");

                entity.Property(e => e.UDtcheque)
                    .HasColumnType("datetime")
                    .HasColumnName("u_dtcheque")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

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

                entity.Property(e => e.UIntgrok).HasColumnName("u_intgrok");

                entity.Property(e => e.UMcam)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_mcam");

                entity.Property(e => e.UMotivtrf)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("u_motivtrf")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UMpag)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("u_mpag")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNib)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_nib")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNocolabo)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("u_nocolabo");

                entity.Property(e => e.UNome)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("u_nome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNomecheq)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("u_nomecheq")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNref)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("u_nref")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UOwirps).HasColumnName("u_owirps");

                entity.Property(e => e.UPdcam)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_pdcam");

                entity.Property(e => e.UPdtot)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_pdtot");

                entity.Property(e => e.URefbanco)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_refbanco")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.USendpay).HasColumnName("u_sendpay");

                entity.Property(e => e.UTotal)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_total");

                entity.Property(e => e.UTotalmoe)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_totalmoe");

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

                entity.Property(e => e.Valorm2)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("valorm2");

                entity.Property(e => e.Virs)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("virs");

                entity.Property(e => e.Zona)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("zona")
                    .HasDefaultValueSql("('')");
            });



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