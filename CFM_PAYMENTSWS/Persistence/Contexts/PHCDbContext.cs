using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.Extensions;
using CFM_PAYMENTSWS.Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

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
        public virtual DbSet<Ow> Ow { get; set; } = null!;
        public virtual DbSet<Pr> Pr { get; set; } = null!;
        public virtual DbSet<Tb> Tb { get; set; } = null!;
        public virtual DbSet<UProvider> UProvider { get; set; } = null!;


        public virtual DbSet<Ft> Ft { get; set; } = null!;
        public virtual DbSet<Ft3> Ft3 { get; set; } = null!;
        public virtual DbSet<Ft2> Ft2 { get; set; } = null!;


        public virtual DbSet<Cc> Cc { get; set; }
        public virtual DbSet<Cl> Cl { get; set; }
        public virtual DbSet<Cl2> Cl2 { get; set; }
        public virtual DbSet<Tsre> Tsre { get; set; }
        public virtual DbSet<Re> Re { get; set; }


        public virtual DbSet<U2bPayments> U2bPayments { get; set; } = null!;


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



            modelBuilder.Entity<Re>(entity =>
            {
                entity.ToTable("Re"); 
                entity.HasKey(e => e.Restamp);
                entity.Property(e => e.Restamp).HasColumnName("restamp");
                entity.Property(e => e.Ccusto).HasColumnName("ccusto");
                entity.Property(e => e.Chdata).HasColumnName("chdata");
                entity.Property(e => e.Contado).HasColumnName("contado");
                entity.Property(e => e.Etotal).HasColumnName("etotal");
                entity.Property(e => e.Etotow).HasColumnName("etotow");
                entity.Property(e => e.Fref).HasColumnName("fref");
                entity.Property(e => e.Local).HasColumnName("local");
                entity.Property(e => e.Memissao).HasColumnName("memissao");
                entity.Property(e => e.Morada).HasColumnName("morada");
                entity.Property(e => e.Ncont).HasColumnName("ncont");
                entity.Property(e => e.Ndoc).HasColumnName("ndoc");
                entity.Property(e => e.Nmdoc).HasColumnName("nmdoc");
                entity.Property(e => e.No).HasColumnName("no");
                entity.Property(e => e.Nome).HasColumnName("nome");
                entity.Property(e => e.Olcodigo).HasColumnName("olcodigo");
                entity.Property(e => e.Ollocal).HasColumnName("ollocal");
                entity.Property(e => e.Ousrdata).HasColumnName("ousrdata");
                entity.Property(e => e.Usrdata).HasColumnName("usrdata");
                entity.Property(e => e.Ousrhora).HasColumnName("ousrhora");
                entity.Property(e => e.Usrhora).HasColumnName("usrhora");
                entity.Property(e => e.Ousrinis).HasColumnName("ousrinis");
                entity.Property(e => e.Usrinis).HasColumnName("usrinis");
                entity.Property(e => e.Process).HasColumnName("process");
                entity.Property(e => e.Rdata).HasColumnName("rdata");
                entity.Property(e => e.Reano).HasColumnName("reano");
                entity.Property(e => e.Rno).HasColumnName("rno");
                entity.Property(e => e.Segmento).HasColumnName("segmento");
                entity.Property(e => e.Telocal).HasColumnName("telocal");
                entity.Property(e => e.Total).HasColumnName("total");
                entity.Property(e => e.Totow).HasColumnName("totow");
                entity.Property(e => e.Procdata).HasColumnName("procdata");
                entity.Property(e => e.Moeda).HasColumnName("moeda");
                entity.Property(e => e.UTransid).HasColumnName("U_Transid");

                entity.Property(e => e.URefps)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("u_refps");
                entity.Property(e => e.UEntps)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("u_entps");

            });

            modelBuilder.Entity<Tsre>(entity =>
            {
                entity.HasKey(e => e.Ndoc)
                    .HasName("pk_tsre")
                    .IsClustered(false);

                entity.ToTable("tsre");

                entity.HasIndex(e => e.Nmdoc, "in_tsre_nmdoc")
                    .IsUnique()
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Tsrestamp, "in_tsre_stamp")
                    .IsUnique()
                    .HasFillFactor(70);

                entity.Property(e => e.Ndoc)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ndoc");
                entity.Property(e => e.Automl).HasColumnName("automl");
                entity.Property(e => e.Clivacaixa).HasColumnName("clivacaixa");
                entity.Property(e => e.Cmcc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("cmcc");
                entity.Property(e => e.Cmccn)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cmccn");
                entity.Property(e => e.Docsimport).HasColumnName("docsimport");
                entity.Property(e => e.Introfac).HasColumnName("introfac");
                entity.Property(e => e.Intropag).HasColumnName("intropag");
                entity.Property(e => e.Intrord).HasColumnName("intrord");
                entity.Property(e => e.Ivacaixa).HasColumnName("ivacaixa");
                entity.Property(e => e.Led)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("led");
                entity.Property(e => e.Manternumero).HasColumnName("manternumero");
                entity.Property(e => e.Marcada).HasColumnName("marcada");
                entity.Property(e => e.Movinc).HasColumnName("movinc");
                entity.Property(e => e.Ndcdesc)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ndcdesc");
                entity.Property(e => e.Ndcno)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("ndcno");
                entity.Property(e => e.Ndidesc)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ndidesc");
                entity.Property(e => e.Ndino)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("ndino");
                entity.Property(e => e.Nmdoc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("nmdoc");
                entity.Property(e => e.Nserierd)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("nserierd");
                entity.Property(e => e.Oldoc)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("oldoc");
                entity.Property(e => e.Ousrdata)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("ousrdata");
                entity.Property(e => e.Ousrhora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ousrhora");
                entity.Property(e => e.Ousrinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ousrinis");
                entity.Property(e => e.Serieox).HasColumnName("serieox");
                entity.Property(e => e.Serierd)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("serierd");
                entity.Property(e => e.Tsrestamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("tsrestamp");
                entity.Property(e => e.UOnlinep).HasColumnName("u_onlinep");
                entity.Property(e => e.Usrdata)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("usrdata");
                entity.Property(e => e.Usrhora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("usrhora");
                entity.Property(e => e.Usrinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("usrinis");
                entity.Property(e => e.Xddesc)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("xddesc");
                entity.Property(e => e.Xdstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("xdstamp");
            });


            modelBuilder.Entity<Cc>(entity =>
            {
                entity.HasKey(e => e.Ccstamp)
                    .HasName("pk_cc")
                    .IsClustered(false);

                entity.HasIndex(e => e.Chstamp, "in_cc_chstamp").HasFillFactor(70);

                entity.HasIndex(e => e.Cm, "in_cc_cm").HasFillFactor(70);

                entity.HasIndex(e => e.Datalc, "in_cc_datalc").HasFillFactor(70);

                entity.HasIndex(e => e.Faccstamp, "in_cc_faccstamp").HasFillFactor(70);

                entity.HasIndex(e => e.Faclstamp, "in_cc_faclstamp").HasFillFactor(70);

                entity.HasIndex(e => e.Facstamp, "in_cc_facstamp").HasFillFactor(70);

                entity.HasIndex(e => e.Ftccstamp, "in_cc_ftccstamp").HasFillFactor(70);

                entity.HasIndex(e => e.Ftstamp, "in_cc_ftstamp").HasFillFactor(70);

                entity.HasIndex(e => e.Lestamp, "in_cc_lestamp").HasFillFactor(70);

                entity.HasIndex(e => e.Lmstamp, "in_cc_lmstamp").HasFillFactor(70);

                entity.HasIndex(e => e.Lrstamp, "in_cc_lrstamp").HasFillFactor(70);

                entity.HasIndex(e => e.No, "in_cc_no").HasFillFactor(70);

                entity.HasIndex(e => e.Nrdoc, "in_cc_nrdoc").HasFillFactor(70);

                entity.HasIndex(e => e.Occstamp, "in_cc_occstamp").HasFillFactor(70);

                entity.HasIndex(e => e.Odstamp, "in_cc_odstamp").HasFillFactor(70);

                entity.HasIndex(e => e.Rdstamp, "in_cc_rdstamp").HasFillFactor(70);

                entity.HasIndex(e => e.Rerevstamp, "in_cc_rerevstamp").HasFillFactor(70);

                entity.HasIndex(e => e.Restamp, "in_cc_restamp").HasFillFactor(70);

                entity.HasIndex(e => e.Tpstamp, "in_cc_tpstamp").HasFillFactor(70);

                entity.HasIndex(e => e.Vendedor, "in_cc_vendedor").HasFillFactor(70);

                entity.Property(e => e.Ccstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("ccstamp");
                entity.Property(e => e.Cambiofixo).HasColumnName("cambiofixo");
                entity.Property(e => e.Cbbno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("cbbno");
                entity.Property(e => e.Ccndoc)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ccndoc");
                entity.Property(e => e.Ccnmdoc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ccnmdoc");
                entity.Property(e => e.Ccusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ccusto");
                entity.Property(e => e.Cecope)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cecope");
                entity.Property(e => e.Chstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("chstamp");
                entity.Property(e => e.Clbanco)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("clbanco");
                entity.Property(e => e.Clcheque)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("clcheque");
                entity.Property(e => e.Cm)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("cm");
                entity.Property(e => e.Cmdesc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cmdesc");
                entity.Property(e => e.Cobrador)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cobrador");
                entity.Property(e => e.Cobradovpaypal).HasColumnName("cobradovpaypal");
                entity.Property(e => e.Cobradovunicre).HasColumnName("cobradovunicre");
                entity.Property(e => e.Cobranca)
                    .HasMaxLength(22)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cobranca");
                entity.Property(e => e.Covezes)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("covezes");
                entity.Property(e => e.Cr)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("cr");
                entity.Property(e => e.Crdesc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("crdesc");
                entity.Property(e => e.Cred)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("cred");
                entity.Property(e => e.Credf)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("credf");
                entity.Property(e => e.Credfm)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("credfm");
                entity.Property(e => e.Credm)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("credm");
                entity.Property(e => e.Datalc)
                    .HasDefaultValueSql("(convert(datetime,'19000101'))")
                    .HasColumnType("datetime")
                    .HasColumnName("datalc");
                entity.Property(e => e.Datasup12)
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))")
                    .HasColumnType("datetime")
                    .HasColumnName("datasup12");
                entity.Property(e => e.Dataven)
                    .HasDefaultValueSql("(convert(datetime,'19000101'))")
                    .HasColumnType("datetime")
                    .HasColumnName("dataven");
                entity.Property(e => e.Deb)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("deb");
                entity.Property(e => e.Debf)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("debf");
                entity.Property(e => e.Debfm)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("debfm");
                entity.Property(e => e.Debm)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("debm");
                entity.Property(e => e.Difcambio)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("difcambio");
                entity.Property(e => e.Difccont)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("difccont");
                entity.Property(e => e.Dispcbb).HasColumnName("dispcbb");
                entity.Property(e => e.Docref)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("docref");
                entity.Property(e => e.Ecred)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ecred");
                entity.Property(e => e.Ecredf)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ecredf");
                entity.Property(e => e.Edeb)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("edeb");
                entity.Property(e => e.Edebf)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("edebf");
                entity.Property(e => e.Edifcambio)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("edifcambio");
                entity.Property(e => e.Edifccont)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("edifccont");
                entity.Property(e => e.Eirsdif)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eirsdif");
                entity.Property(e => e.Eivacatdif)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivacatdif");
                entity.Property(e => e.Eivacativado)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivacativado");
                entity.Property(e => e.Eivacatreg)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivacatreg");
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
                entity.Property(e => e.Escvtmp)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("escvtmp");
                entity.Property(e => e.Estab)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("estab");
                entity.Property(e => e.Evalch)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evalch");
                entity.Property(e => e.Evalre)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evalre");
                entity.Property(e => e.Evirs)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evirs");
                entity.Property(e => e.Evirsreg)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evirsreg");
                entity.Property(e => e.Evtmp)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evtmp");
                entity.Property(e => e.Faccstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("faccstamp");
                entity.Property(e => e.Faclstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("faclstamp");
                entity.Property(e => e.Facstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("facstamp");
                entity.Property(e => e.Fmarcada).HasColumnName("fmarcada");
                entity.Property(e => e.Formapag)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("formapag");
                entity.Property(e => e.Fref)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("fref");
                entity.Property(e => e.Ftccstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("ftccstamp");
                entity.Property(e => e.Ftfno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ftfno");
                entity.Property(e => e.Ftndoc)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ftndoc");
                entity.Property(e => e.Ftstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("ftstamp");
                entity.Property(e => e.Incobra).HasColumnName("incobra");
                entity.Property(e => e.Intid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("intid");
                entity.Property(e => e.Irsdif)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("irsdif");
                entity.Property(e => e.Ivacatdif)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivacatdif");
                entity.Property(e => e.Ivacativado)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivacativado");
                entity.Property(e => e.Ivacatreg)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivacatreg");
                entity.Property(e => e.Ivatx1)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ivatx1");
                entity.Property(e => e.Ivatx2)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ivatx2");
                entity.Property(e => e.Ivatx3)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ivatx3");
                entity.Property(e => e.Ivatx4)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ivatx4");
                entity.Property(e => e.Ivatx5)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ivatx5");
                entity.Property(e => e.Ivatx6)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ivatx6");
                entity.Property(e => e.Ivatx7)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ivatx7");
                entity.Property(e => e.Ivatx8)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ivatx8");
                entity.Property(e => e.Ivatx9)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ivatx9");
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
                entity.Property(e => e.Lestamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("lestamp");
                entity.Property(e => e.Lmstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("lmstamp");
                entity.Property(e => e.Lrstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("lrstamp");
                entity.Property(e => e.Marcada).HasColumnName("marcada");
                entity.Property(e => e.Mivacatdif)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("mivacatdif");
                entity.Property(e => e.Mivacativado)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("mivacativado");
                entity.Property(e => e.Mivacatreg)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("mivacatreg");
                entity.Property(e => e.Moeda)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("moeda");
                entity.Property(e => e.Mvalre)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("mvalre");
                entity.Property(e => e.Ncusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ncusto");
                entity.Property(e => e.No)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("no");
                entity.Property(e => e.Nome)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("nome");
                entity.Property(e => e.Nrdoc)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("nrdoc");
                entity.Property(e => e.Obs)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("obs");
                entity.Property(e => e.Obscob)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("obscob");
                entity.Property(e => e.Occstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("occstamp");
                entity.Property(e => e.Odstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("odstamp");
                entity.Property(e => e.Operext).HasColumnName("operext");
                entity.Property(e => e.Origem)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("origem");
                entity.Property(e => e.Ousrdata)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("ousrdata");
                entity.Property(e => e.Ousrhora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ousrhora");
                entity.Property(e => e.Ousrinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ousrinis");
                entity.Property(e => e.Pais)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("pais");
                entity.Property(e => e.Rdstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("rdstamp");
                entity.Property(e => e.Recian)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("recian");
                entity.Property(e => e.Recibado).HasColumnName("recibado");
                entity.Property(e => e.Recindoc)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("recindoc");
                entity.Property(e => e.Recino)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("recino");
                entity.Property(e => e.Reexgiva).HasColumnName("reexgiva");
                entity.Property(e => e.Rerevstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("rerevstamp");
                entity.Property(e => e.Restamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("restamp");
                entity.Property(e => e.Rota)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("rota");
                entity.Property(e => e.Segmento)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("segmento");
                entity.Property(e => e.Situacao)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("situacao");
                entity.Property(e => e.Tipo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("tipo");
                entity.Property(e => e.Tpdesc)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("tpdesc");
                entity.Property(e => e.Tpstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("tpstamp");
                entity.Property(e => e.Usrdata)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("usrdata");
                entity.Property(e => e.Usrhora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("usrhora");
                entity.Property(e => e.Usrinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("usrinis");
                entity.Property(e => e.Valch)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("valch");
                entity.Property(e => e.Valre)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("valre");
                entity.Property(e => e.Vendedor)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("vendedor");
                entity.Property(e => e.Vendnm)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("vendnm");
                entity.Property(e => e.Virs)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("virs");
                entity.Property(e => e.Virsreg)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("virsreg");
                entity.Property(e => e.Vtmp)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("vtmp");
                entity.Property(e => e.Zona)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("zona");
            });


            modelBuilder.Entity<Cl>(entity =>
            {
                entity.HasKey(e => new { e.No, e.Estab })
                    .HasName("pk_cl")
                    .IsClustered(false);

                entity.HasIndex(e => new { e.Nome, e.Nome2, e.No, e.Estab, e.Clstamp }, "in_cl_cllist").HasFillFactor(70);

                entity.HasIndex(e => e.Ncont, "in_cl_ncont").HasFillFactor(70);

                entity.HasIndex(e => e.No, "in_cl_no").HasFillFactor(70);

                entity.HasIndex(e => e.Nome, "in_cl_nome").HasFillFactor(70);

                entity.HasIndex(e => e.Clstamp, "in_cl_stamp")
                    .IsUnique()
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Vendedor, "in_cl_vendedor").HasFillFactor(70);

                entity.Property(e => e.No)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("no");
                entity.Property(e => e.Estab)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("estab");
                entity.Property(e => e.Acc)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("acc");
                entity.Property(e => e.Acmfact)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("acmfact");
                entity.Property(e => e.Addd).HasColumnName("addd");
                entity.Property(e => e.Agno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("agno");
                entity.Property(e => e.Alimite)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("alimite");
                entity.Property(e => e.Area)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("area");
                entity.Property(e => e.Autofact).HasColumnName("autofact");
                entity.Property(e => e.Autorizacaoactiva).HasColumnName("autorizacaoactiva");
                entity.Property(e => e.Bic)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("bic");
                entity.Property(e => e.Bidata)
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))")
                    .HasColumnType("datetime")
                    .HasColumnName("bidata");
                entity.Property(e => e.Bilocal)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("bilocal");
                entity.Property(e => e.Bino)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("bino");
                entity.Property(e => e.Bizzaddress)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("bizzaddress");
                entity.Property(e => e.Bizzproto)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("bizzproto");
                entity.Property(e => e.Blck).HasColumnName("blck");
                entity.Property(e => e.C1email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("c1email");
                entity.Property(e => e.C1fax)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("c1fax");
                entity.Property(e => e.C1func)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("c1func");
                entity.Property(e => e.C1tele)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("c1tele");
                entity.Property(e => e.C2email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("c2email");
                entity.Property(e => e.C2fax)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("c2fax");
                entity.Property(e => e.C2func)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("c2func");
                entity.Property(e => e.C2tacto)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("c2tacto");
                entity.Property(e => e.C2tele)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("c2tele");
                entity.Property(e => e.C3email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("c3email");
                entity.Property(e => e.C3fax)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("c3fax");
                entity.Property(e => e.C3func)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("c3func");
                entity.Property(e => e.C3tacto)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("c3tacto");
                entity.Property(e => e.C3tele)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("c3tele");
                entity.Property(e => e.Cancpos).HasColumnName("cancpos");
                entity.Property(e => e.Carr).HasColumnName("carr");
                entity.Property(e => e.Cass)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cass");
                entity.Property(e => e.Ccadmin).HasColumnName("ccadmin");
                entity.Property(e => e.Ccusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ccusto");
                entity.Property(e => e.Classe)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("classe");
                entity.Property(e => e.Clifactor).HasColumnName("clifactor");
                entity.Property(e => e.Clinica).HasColumnName("clinica");
                entity.Property(e => e.Clivd).HasColumnName("clivd");
                entity.Property(e => e.Clstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("clstamp");
                entity.Property(e => e.Cobemail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cobemail");
                entity.Property(e => e.Cobfax)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cobfax");
                entity.Property(e => e.Cobfunc)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cobfunc");
                entity.Property(e => e.Cobnao).HasColumnName("cobnao");
                entity.Property(e => e.Cobrador)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cobrador");
                entity.Property(e => e.Cobranca)
                    .HasMaxLength(22)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cobranca");
                entity.Property(e => e.Cobtacto)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cobtacto");
                entity.Property(e => e.Cobtele)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cobtele");
                entity.Property(e => e.Codfornecedor)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("codfornecedor");
                entity.Property(e => e.Codmotiseimp)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("codmotiseimp");
                entity.Property(e => e.Codpost)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("codpost");
                entity.Property(e => e.Codprovincia)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("codprovincia");
                entity.Property(e => e.Consfinal).HasColumnName("consfinal");
                entity.Property(e => e.Conta)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("conta");
                entity.Property(e => e.Contaacer)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("contaacer");
                entity.Property(e => e.Contaainc)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("contaainc");
                entity.Property(e => e.Contacto)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("contacto");
                entity.Property(e => e.Contadivinc)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("contadivinc");
                entity.Property(e => e.Contado)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("contado");
                entity.Property(e => e.Contafac)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("contafac");
                entity.Property(e => e.Contalet)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("contalet");
                entity.Property(e => e.Contaletdes)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("contaletdes");
                entity.Property(e => e.Contaletsac)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("contaletsac");
                entity.Property(e => e.Contamovinc)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("contamovinc");
                entity.Property(e => e.Contatit)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("contatit");
                entity.Property(e => e.Cw).HasColumnName("cw");
                entity.Property(e => e.Datasdd)
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))")
                    .HasColumnType("datetime")
                    .HasColumnName("datasdd");
                entity.Property(e => e.Descarga)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("descarga");
                entity.Property(e => e.Desccmb)
                    .HasColumnType("numeric(10, 3)")
                    .HasColumnName("desccmb");
                entity.Property(e => e.Descloj)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("descloj");
                entity.Property(e => e.Desconto)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("desconto");
                entity.Property(e => e.Descpp)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("descpp");
                entity.Property(e => e.Descregiva)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("descregiva");
                entity.Property(e => e.Dformacao).HasColumnName("dformacao");
                entity.Property(e => e.Dfront).HasColumnName("dfront");
                entity.Property(e => e.Diaspag)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("diaspag");
                entity.Property(e => e.Did).HasColumnName("did");
                entity.Property(e => e.Distrito)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("distrito");
                entity.Property(e => e.Dqtt).HasColumnName("dqtt");
                entity.Property(e => e.Dqttval)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("dqttval");
                entity.Property(e => e.Dsuporte).HasColumnName("dsuporte");
                entity.Property(e => e.Dteam).HasColumnName("dteam");
                entity.Property(e => e.Eacmfact)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eacmfact");
                entity.Property(e => e.Eag).HasColumnName("eag");
                entity.Property(e => e.Eancl)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("eancl");
                entity.Property(e => e.Ecoisento).HasColumnName("ecoisento");
                entity.Property(e => e.Ediexp).HasColumnName("ediexp");
                entity.Property(e => e.Eem).HasColumnName("eem");
                entity.Property(e => e.Efl).HasColumnName("efl");
                entity.Property(e => e.Eid).HasColumnName("eid");
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("email");
                entity.Property(e => e.Emno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("emno");
                entity.Property(e => e.Encm)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("encm");
                entity.Property(e => e.Encmdesc)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("encmdesc");
                entity.Property(e => e.Encrpin)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("encrpin");
                entity.Property(e => e.Eplafond)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eplafond");
                entity.Property(e => e.Erentval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("erentval");
                entity.Property(e => e.Esaldlet)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("esaldlet");
                entity.Property(e => e.Esaldo)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("esaldo");
                entity.Property(e => e.Excm)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("excm");
                entity.Property(e => e.Excmdesc)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("excmdesc");
                entity.Property(e => e.Exporpos).HasColumnName("exporpos");
                entity.Property(e => e.Fax)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("fax");
                entity.Property(e => e.Filtrast).HasColumnName("filtrast");
                entity.Property(e => e.Flestab)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("flestab");
                entity.Property(e => e.Flno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("flno");
                entity.Property(e => e.Fref)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("fref");
                entity.Property(e => e.Ftdatasmr).HasColumnName("ftdatasmr");
                entity.Property(e => e.Ftdiasmr).HasColumnName("ftdiasmr");
                entity.Property(e => e.Ftidbi).HasColumnName("ftidbi");
                entity.Property(e => e.Ftidcob).HasColumnName("ftidcob");
                entity.Property(e => e.Ftidcont).HasColumnName("ftidcont");
                entity.Property(e => e.Ftidcontacto).HasColumnName("ftidcontacto");
                entity.Property(e => e.Ftidnac).HasColumnName("ftidnac");
                entity.Property(e => e.Ftidnome).HasColumnName("ftidnome");
                entity.Property(e => e.Ftidutente).HasColumnName("ftidutente");
                entity.Property(e => e.Ftmrtot).HasColumnName("ftmrtot");
                entity.Property(e => e.Ftndias).HasColumnName("ftndias");
                entity.Property(e => e.Ftnid).HasColumnName("ftnid");
                entity.Property(e => e.Ftumamr).HasColumnName("ftumamr");
                entity.Property(e => e.Fuels)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("fuels");
                entity.Property(e => e.Gaecstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("gaecstamp");
                entity.Property(e => e.Gaenome)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("gaenome");
                entity.Property(e => e.Geramb).HasColumnName("geramb");
                entity.Property(e => e.Glncl)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("glncl");
                entity.Property(e => e.Iban)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("iban");
                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("id");
                entity.Property(e => e.Idno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("idno");
                entity.Property(e => e.Iectisento).HasColumnName("iectisento");
                entity.Property(e => e.Imagem)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("imagem");
                entity.Property(e => e.Inactivo).HasColumnName("inactivo");
                entity.Property(e => e.Isperson).HasColumnName("isperson");
                entity.Property(e => e.Lang)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("lang");
                entity.Property(e => e.Lmlt)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("lmlt");
                entity.Property(e => e.Local)
                    .HasMaxLength(43)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("local");
                entity.Property(e => e.Localentrega)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("localentrega");
                entity.Property(e => e.Ltyp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ltyp");
                entity.Property(e => e.Marcada).HasColumnName("marcada");
                entity.Property(e => e.Matric)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("matric");
                entity.Property(e => e.Mesesnaopag)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("mesesnaopag");
                entity.Property(e => e.Moeda)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("moeda");
                entity.Property(e => e.Morada)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("morada");
                entity.Property(e => e.Motiseimp)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("motiseimp");
                entity.Property(e => e.Naoencomenda).HasColumnName("naoencomenda");
                entity.Property(e => e.Naomail).HasColumnName("naomail");
                entity.Property(e => e.Naood).HasColumnName("naood");
                entity.Property(e => e.Nascimento)
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))")
                    .HasColumnType("datetime")
                    .HasColumnName("nascimento");
                entity.Property(e => e.Naturalid)
                    .HasMaxLength(17)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("naturalid");
                entity.Property(e => e.Ncont)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ncont");
                entity.Property(e => e.Ncusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ncusto");
                entity.Property(e => e.Nib)
                    .HasMaxLength(28)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("nib");
                entity.Property(e => e.Niec)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("niec");
                entity.Property(e => e.Nocredit).HasColumnName("nocredit");
                entity.Property(e => e.Nome)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("nome");
                entity.Property(e => e.Nome2)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("nome2");
                entity.Property(e => e.Ntcm)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ntcm");
                entity.Property(e => e.Numautorizacaosdd)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("numautorizacaosdd");
                entity.Property(e => e.Numcontrepres)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("numcontrepres");
                entity.Property(e => e.Numseqaut)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("numseqaut");
                entity.Property(e => e.Obs)
                    .HasMaxLength(240)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("obs");
                entity.Property(e => e.Obsdoc)
                    .HasDefaultValueSql("('')")
                    .HasColumnType("text")
                    .HasColumnName("obsdoc");
                entity.Property(e => e.Odatraso)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("odatraso");
                entity.Property(e => e.Odo).HasColumnName("odo");
                entity.Property(e => e.Ollocal)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ollocal");
                entity.Property(e => e.Operext).HasColumnName("operext");
                entity.Property(e => e.Ousrdata)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("ousrdata");
                entity.Property(e => e.Ousrhora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ousrhora");
                entity.Property(e => e.Ousrinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ousrinis");
                entity.Property(e => e.Pagamento)
                    .HasMaxLength(28)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("pagamento");
                entity.Property(e => e.Pais)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("pais");
                entity.Property(e => e.Paramr).HasColumnName("paramr");
                entity.Property(e => e.Particular).HasColumnName("particular");
                entity.Property(e => e.Passaporte)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("passaporte");
                entity.Property(e => e.Pcktsyncdate)
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))")
                    .HasColumnType("datetime")
                    .HasColumnName("pcktsyncdate");
                entity.Property(e => e.Pcktsynctime)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("pcktsynctime");
                entity.Property(e => e.Pin)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("pin");
                entity.Property(e => e.Plafond)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("plafond");
                entity.Property(e => e.Pncont)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("pncont");
                entity.Property(e => e.Preco)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("preco");
                entity.Property(e => e.Pscm)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("pscm");
                entity.Property(e => e.Pscmdesc)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("pscmdesc");
                entity.Property(e => e.Ptcm)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ptcm");
                entity.Property(e => e.Ptcmdesc)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ptcmdesc");
                entity.Property(e => e.Radicaltipoemp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("radicaltipoemp");
                entity.Property(e => e.Rbal)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("rbal");
                entity.Property(e => e.Recdocdig).HasColumnName("recdocdig");
                entity.Property(e => e.Refcli)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("refcli");
                entity.Property(e => e.Rentval)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("rentval");
                entity.Property(e => e.Repl).HasColumnName("repl");
                entity.Property(e => e.Rota)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("rota");
                entity.Property(e => e.Saldlet)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("saldlet");
                entity.Property(e => e.Saldo)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("saldo");
                entity.Property(e => e.Saldoini)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("saldoini");
                entity.Property(e => e.Saldopa)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("saldopa");
                entity.Property(e => e.Segmento)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("segmento");
                entity.Property(e => e.Sepacode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("sepacode");
                entity.Property(e => e.Shop)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("shop");
                entity.Property(e => e.Site)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("site");
                entity.Property(e => e.Statuspda)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("statuspda");
                entity.Property(e => e.Tabiva)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("tabiva");
                entity.Property(e => e.Taxairs)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("taxairs");
                entity.Property(e => e.Tbprcod)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("tbprcod");
                entity.Property(e => e.Telefone)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("telefone");
                entity.Property(e => e.Temcred).HasColumnName("temcred");
                entity.Property(e => e.Temftglob).HasColumnName("temftglob");
                entity.Property(e => e.Tipo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("tipo");
                entity.Property(e => e.Tipodesc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("tipodesc");
                entity.Property(e => e.Tlmvl)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("tlmvl");
                entity.Property(e => e.Tpdesc)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("tpdesc");
                entity.Property(e => e.Tpstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("tpstamp");
                entity.Property(e => e.Track)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("track");
                entity.Property(e => e.Tracknr)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("tracknr");
                entity.Property(e => e.Txftdata)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("txftdata");
                entity.Property(e => e.Txftdias)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("txftdias");
                entity.Property(e => e.Txftidbi)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("txftidbi");
                entity.Property(e => e.Txftidcob)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("txftidcob");
                entity.Property(e => e.Txftidcont)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("txftidcont");
                entity.Property(e => e.Txftidcontacto)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("txftidcontacto");
                entity.Property(e => e.Txftidnac)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("txftidnac");
                entity.Property(e => e.Txftidnome)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("txftidnome");
                entity.Property(e => e.Txftidutente)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("txftidutente");
                entity.Property(e => e.Txftmrtot)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("txftmrtot");
                entity.Property(e => e.Txftndias)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("txftndias");
                entity.Property(e => e.Txftnid)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("txftnid");
                entity.Property(e => e.Txirspersonalizada).HasColumnName("txirspersonalizada");
                entity.Property(e => e.Url)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("url");
                entity.Property(e => e.Usrdata)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("usrdata");
                entity.Property(e => e.Usrhora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("usrhora");
                entity.Property(e => e.Usrinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("usrinis");
                entity.Property(e => e.Vencimento)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("vencimento");
                entity.Property(e => e.Vendedor)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("vendedor");
                entity.Property(e => e.Vendnm)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("vendnm");
                entity.Property(e => e.Zncm)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("zncm");
                entity.Property(e => e.Znregiao)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("znregiao");
                entity.Property(e => e.Zona)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("zona");
            });

            modelBuilder.Entity<Cl2>(entity =>
            {
                entity.HasKey(e => e.Cl2stamp)
                    .HasName("pk_cl2")
                    .IsClustered(false);

                entity.Property(e => e.Cl2stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("cl2stamp");
                entity.Property(e => e.Adcsepaativa).HasColumnName("adcsepaativa");
                entity.Property(e => e.Cadmintipo1)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cadmintipo1");
                entity.Property(e => e.Cadmintipo1stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("cadmintipo1stamp");
                entity.Property(e => e.Cadmintipo2)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cadmintipo2");
                entity.Property(e => e.Cadmintipo2stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("cadmintipo2stamp");
                entity.Property(e => e.Cadmintipo3)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cadmintipo3");
                entity.Property(e => e.Cadmintipo3stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("cadmintipo3stamp");
                entity.Property(e => e.Cadmintipo4)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("cadmintipo4");
                entity.Property(e => e.Cadmintipo4stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength()
                    .HasColumnName("cadmintipo4stamp");
                entity.Property(e => e.Cativaperc)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("cativaperc");
                entity.Property(e => e.Clivacaixa).HasColumnName("clivacaixa");
                entity.Property(e => e.Cobrecsede).HasColumnName("cobrecsede");
                entity.Property(e => e.Codendereco)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("codendereco");
                entity.Property(e => e.Codpais)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("codpais");
                entity.Property(e => e.Dbpm).HasColumnName("dbpm");
                entity.Property(e => e.Descpais)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("descpais");
                entity.Property(e => e.Dgrelhas).HasColumnName("dgrelhas");
                entity.Property(e => e.Dlogin)
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))")
                    .HasColumnType("datetime")
                    .HasColumnName("dlogin");
                entity.Property(e => e.Doceletronicos).HasColumnName("doceletronicos");
                entity.Property(e => e.Egarapa)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("egarapa");
                entity.Property(e => e.Egargrupo)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("egargrupo");
                entity.Property(e => e.Egaropdes)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("egaropdes");
                entity.Property(e => e.Egaropera)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("egaropera");
                entity.Property(e => e.Egarpgl)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("egarpgl");
                entity.Property(e => e.Egarprod)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("egarprod");
                entity.Property(e => e.Estabmod)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("estabmod");
                entity.Property(e => e.Forgotdate)
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))")
                    .HasColumnType("datetime")
                    .HasColumnName("forgotdate");
                entity.Property(e => e.Forgotid)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("forgotid");
                entity.Property(e => e.Hlogin)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("hlogin");
                entity.Property(e => e.Isb2b).HasColumnName("isb2b");
                entity.Property(e => e.Latitude)
                    .HasColumnType("numeric(10, 6)")
                    .HasColumnName("latitude");
                entity.Property(e => e.Longitude)
                    .HasColumnType("numeric(10, 6)")
                    .HasColumnName("longitude");
                entity.Property(e => e.Marcada).HasColumnName("marcada");
                entity.Property(e => e.Monitignios).HasColumnName("monitignios");
                entity.Property(e => e.Nifvalidado).HasColumnName("nifvalidado");
                entity.Property(e => e.Nomemod)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("nomemod");
                entity.Property(e => e.Nomod)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("nomod");
                entity.Property(e => e.Ousrdata)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("ousrdata");
                entity.Property(e => e.Ousrhora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ousrhora");
                entity.Property(e => e.Ousrinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("ousrinis");
                entity.Property(e => e.Passpais)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("passpais");
                entity.Property(e => e.Passpaisdesc)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("passpaisdesc");
                entity.Property(e => e.Pwportal)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("pwportal");
                entity.Property(e => e.Refmbndias)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("refmbndias");
                entity.Property(e => e.Refmbtpdata)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("refmbtpdata");
                entity.Property(e => e.Refmbusavalidade).HasColumnName("refmbusavalidade");
                entity.Property(e => e.Retarrat)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("retarrat");
                entity.Property(e => e.Tdocidcod)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("tdocidcod");
                entity.Property(e => e.Tdocidenif).HasColumnName("tdocidenif");
                entity.Property(e => e.Termsconditions).HasColumnName("termsconditions");
                entity.Property(e => e.Userid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("userid");
                entity.Property(e => e.Usrdata)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("usrdata");
                entity.Property(e => e.Usrhora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("usrhora");
                entity.Property(e => e.Usrinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("usrinis");
                entity.Property(e => e.Validadecartao)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasColumnName("validadecartao");
            });


            modelBuilder.Entity<Ft2>(entity =>
            {
                entity.HasKey(e => e.Ft2stamp)
                    .HasName("pk_ft2")
                    .IsClustered(false);

                entity.ToTable("ft2");

                entity.Property(e => e.Ft2stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ft2stamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.URefps2)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("u_refps2")
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


            modelBuilder.Entity<Ft>(entity =>
            {
                entity.HasKey(e => new { e.Ndoc, e.Fno, e.Ftano })
                    .HasName("pk_ft")
                    .IsClustered(false);

                entity.ToTable("ft");

                entity.HasIndex(e => e.Anulado, "in_ft_anulado")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Fdata, "in_ft_fdata")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Nome, "in_ft_ftlist")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.No, "in_ft_no")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Nome, "in_ft_nome")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Ftstamp, "in_ft_stamp")
                    .IsUnique()
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Tipodoc, "in_ft_tipo_anula_ft_ndoc")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Tipodoc, "in_ft_tipodoc")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Vendedor, "in_ft_vendedor")
                    .HasFillFactor(70);

                entity.Property(e => e.Ndoc)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ndoc");

                entity.Property(e => e.Fno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("fno");

                entity.Property(e => e.Ftano)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("ftano");

                entity.Property(e => e.Anulado).HasColumnName("anulado");

                entity.Property(e => e.Bidata)
                    .HasColumnType("datetime")
                    .HasColumnName("bidata")
                    .HasDefaultValueSql("(convert(datetime,'19000101'))");

                entity.Property(e => e.Bilocal)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("bilocal")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Codpost)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("codpost")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Descc)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("descc");

                entity.Property(e => e.Descm)
                    .HasColumnType("numeric(13, 3)")
                    .HasColumnName("descm");

                entity.Property(e => e.Fdata)
                    .HasColumnType("datetime")
                    .HasColumnName("fdata")
                    .HasDefaultValueSql("(convert(datetime,'19000101'))");

                entity.Property(e => e.Ftid)
                    .HasColumnType("numeric(12, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ftid");

                entity.Property(e => e.Ftstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ftstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Local)
                    .HasMaxLength(43)
                    .IsUnicode(false)
                    .HasColumnName("local")
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

                entity.Property(e => e.Nmdoc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("nmdoc")
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

                entity.Property(e => e.Telefone)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("telefone")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tipodoc)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("tipodoc");

                entity.Property(e => e.Tmiliq)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("tmiliq");

                entity.Property(e => e.Total)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("total");

                entity.Property(e => e.Ttiliq)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ttiliq");

                entity.Property(e => e.Ttiva)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ttiva");

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

                entity.Property(e => e.Vendedor)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("vendedor");

                entity.Property(e => e.Vendnm)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("vendnm")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Zona)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("zona")
                    .HasDefaultValueSql("('')");
            });


            modelBuilder.Entity<Ft3>(entity =>
            {
                entity.HasKey(e => e.Ft3stamp)
                    .HasName("pk_ft3")
                    .IsClustered(false);

                entity.ToTable("ft3");

                entity.HasIndex(e => new { e.Seriecode, e.Docno }, "in_ft3_docno")
                    .IsUnique()
                    .HasFillFactor(70);

                entity.Property(e => e.Ft3stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ft3stamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Anexo40)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("anexo40")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Anexo41)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("anexo41")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Anularetif)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("anularetif")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Anuldata)
                    .HasColumnType("datetime")
                    .HasColumnName("anuldata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Anulhora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("anulhora")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Anulinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("anulinis")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Atcud)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("atcud")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Barcode)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("barcode")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.C2codpais)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("c2codpais")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.C2descpais)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("c2descpais")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.C2distrito)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("c2distrito")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cadmintipo1)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cadmintipo1")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cadmintipo1stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cadmintipo1stamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Cadmintipo2)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cadmintipo2")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cadmintipo2stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cadmintipo2stamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Cadmintipo3)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cadmintipo3")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cadmintipo3stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cadmintipo3stamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Cadmintipo4)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cadmintipo4")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cadmintipo4stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cadmintipo4stamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Cativaperc)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("cativaperc");

                entity.Property(e => e.Cobradovmbway).HasColumnName("cobradovmbway");

                entity.Property(e => e.Codendereco)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("codendereco")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Codmotivreg)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("codmotivreg");

                entity.Property(e => e.Codpais)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("codpais")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Contingencia)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("contingencia");

                entity.Property(e => e.Descpais)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("descpais")
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

                entity.Property(e => e.Distrito)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("distrito")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Docno)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("docno");

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

                entity.Property(e => e.Eivacativado)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivacativado");

                entity.Property(e => e.Etotgroj)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("etotgroj");

                entity.Property(e => e.Etotrc)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("etotrc");

                entity.Property(e => e.Fiscalcode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("fiscalcode")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fttxirs)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("fttxirs");

                entity.Property(e => e.Invoicenoori)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("invoicenoori")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Invoiceyear)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("invoiceyear");

                entity.Property(e => e.Ivacativado)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivacativado");

                entity.Property(e => e.Latitude)
                    .HasColumnType("numeric(10, 6)")
                    .HasColumnName("latitude");

                entity.Property(e => e.Latitudecarga)
                    .HasColumnType("numeric(10, 6)")
                    .HasColumnName("latitudecarga");

                entity.Property(e => e.Longitude)
                    .HasColumnType("numeric(10, 6)")
                    .HasColumnName("longitude");

                entity.Property(e => e.Longitudecarga)
                    .HasColumnType("numeric(10, 6)")
                    .HasColumnName("longitudecarga");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Meiotranscv)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("meiotranscv");

                entity.Property(e => e.Mivacativado)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("mivacativado");

                entity.Property(e => e.Motanul)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("motanul")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Motivreg)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("motivreg")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Motivregoutro)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("motivregoutro")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Motorista)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("motorista")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Motretif)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("motretif")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Mtotgroj)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("mtotgroj");

                entity.Property(e => e.Mtotrc)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("mtotrc");

                entity.Property(e => e.Naoisenta).HasColumnName("naoisenta");

                entity.Property(e => e.Oricae)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("oricae")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Oridata)
                    .HasColumnType("datetime")
                    .HasColumnName("oridata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Orihora)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("orihora")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Oriinis)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("oriinis")
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

                entity.Property(e => e.Planoonline).HasColumnName("planoonline");

                entity.Property(e => e.Ppallowpaynow).HasColumnName("ppallowpaynow");

                entity.Property(e => e.Ppallowsubscription).HasColumnName("ppallowsubscription");

                entity.Property(e => e.Ppbillingagreement)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ppbillingagreement")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ppperiodicidade)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ppperiodicidade")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ppperiodo)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ppperiodo");

                entity.Property(e => e.Ppprofileid)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ppprofileid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ppsbstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ppsbstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Pptransactionid)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("pptransactionid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Refmbdtvalidade)
                    .HasColumnType("datetime")
                    .HasColumnName("refmbdtvalidade")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Region)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("region")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Seriecode)
                    .HasColumnType("numeric(12, 0)")
                    .HasColumnName("seriecode");

                entity.Property(e => e.Subscritovpaypal).HasColumnName("subscritovpaypal");

                entity.Property(e => e.Taxfree).HasColumnName("taxfree");

                entity.Property(e => e.Taxpointdt)
                    .HasColumnType("datetime")
                    .HasColumnName("taxpointdt")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Tfdatanasc)
                    .HasColumnType("datetime")
                    .HasColumnName("tfdatanasc")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Tfdocid)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("tfdocid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tfdocnum)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("tfdocnum")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tfdocpais)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("tfdocpais")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tfdocpaisdesc)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("tfdocpaisdesc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tfdoctipo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tfdoctipo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tfeivain1)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivain1");

                entity.Property(e => e.Tfeivain2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivain2");

                entity.Property(e => e.Tfeivain3)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivain3");

                entity.Property(e => e.Tfeivain4)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivain4");

                entity.Property(e => e.Tfeivain5)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivain5");

                entity.Property(e => e.Tfeivain6)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivain6");

                entity.Property(e => e.Tfeivain7)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivain7");

                entity.Property(e => e.Tfeivain8)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivain8");

                entity.Property(e => e.Tfeivain9)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivain9");

                entity.Property(e => e.Tfeivav1)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivav1");

                entity.Property(e => e.Tfeivav2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivav2");

                entity.Property(e => e.Tfeivav3)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivav3");

                entity.Property(e => e.Tfeivav4)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivav4");

                entity.Property(e => e.Tfeivav5)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivav5");

                entity.Property(e => e.Tfeivav6)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivav6");

                entity.Property(e => e.Tfeivav7)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivav7");

                entity.Property(e => e.Tfeivav8)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivav8");

                entity.Property(e => e.Tfeivav9)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfeivav9");

                entity.Property(e => e.Tfgrossamount)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfgrossamount");

                entity.Property(e => e.Tfivatx1)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("tfivatx1");

                entity.Property(e => e.Tfivatx2)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("tfivatx2");

                entity.Property(e => e.Tfivatx3)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("tfivatx3");

                entity.Property(e => e.Tfivatx4)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("tfivatx4");

                entity.Property(e => e.Tfivatx5)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("tfivatx5");

                entity.Property(e => e.Tfivatx6)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("tfivatx6");

                entity.Property(e => e.Tfivatx7)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("tfivatx7");

                entity.Property(e => e.Tfivatx8)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("tfivatx8");

                entity.Property(e => e.Tfivatx9)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("tfivatx9");

                entity.Property(e => e.Tfpaisori)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("tfpaisori");

                entity.Property(e => e.Tfrefund)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("tfrefund");

                entity.Property(e => e.Tfserviceid)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("tfserviceid");

                entity.Property(e => e.Totgroj)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("totgroj");

                entity.Property(e => e.Totrc)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("totrc");

                entity.Property(e => e.Tpaaccountperiodnum)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("tpaaccountperiodnum")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpaamountconverted)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("tpaamountconverted")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpacommissionsignal)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("tpacommissionsignal")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpacommissionvalue)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("tpacommissionvalue")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpaissuername)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tpaissuername")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpamessagenum)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("tpamessagenum")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpamreceipt)
                    .HasColumnType("text")
                    .HasColumnName("tpamreceipt")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpapan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tpapan")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpaposid)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("tpaposid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpapurchase).HasColumnName("tpapurchase");

                entity.Property(e => e.Tpareceipt)
                    .HasColumnType("text")
                    .HasColumnName("tpareceipt")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpareceiptformat)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("tpareceiptformat");

                entity.Property(e => e.Tparesponsedatetime)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("tparesponsedatetime")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpasan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tpasan")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpatextforclientreceipt)
                    .HasColumnType("text")
                    .HasColumnName("tpatextforclientreceipt")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpatransactionseqnum)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("tpatransactionseqnum")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpatype)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("tpatype")
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

                entity.Property(e => e.Walletreceiptid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("walletreceiptid")
                    .HasDefaultValueSql("('')");
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


            modelBuilder.Entity<Pr>(entity =>
            {
                entity.HasKey(e => e.Prstamp)
                    .HasName("pk_pr")
                    .IsClustered(false);

                entity.ToTable("pr");

                entity.HasIndex(e => e.No, "in_pr_no")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Nome, "in_pr_nome")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.No, e.Pago, e.Dpago, e.Resestr, e.Estab, e.Czonag, e.Data, e.Prstamp }, "in_pr_pago")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.Nome, e.No, e.Recibo, e.Data, e.Prstamp }, "in_pr_prlist")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Recibo, "in_pr_recibo")
                    .HasFillFactor(80);

                entity.Property(e => e.Prstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("prstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Abrsit)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("abrsit")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Acidente)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("acidente");

                entity.Property(e => e.Adseevalor)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("adseevalor");

                entity.Property(e => e.Ano)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("ano");

                entity.Property(e => e.Area)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("area")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Bairro)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("bairro")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Banco)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("banco")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Basedia)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("basedia");

                entity.Property(e => e.Basemes)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("basemes");

                entity.Property(e => e.Bonus)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("bonus");

                entity.Property(e => e.Caixa)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("caixa")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ccategoria)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ccategoria")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ccprofiss)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ccprofiss")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ccusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ccusto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cgaevalor)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("cgaevalor");

                entity.Property(e => e.Cgass).HasColumnName("cgass");

                entity.Property(e => e.Cheque)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cheque")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Codigo)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("codigo");

                entity.Property(e => e.Codrdtrab)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("codrdtrab")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Codtcontr)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("codtcontr")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Col9)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("col9")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Contabil).HasColumnName("contabil");

                entity.Property(e => e.Contado)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("contado");

                entity.Property(e => e.Cprofiss)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cprofiss")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Curso)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("curso")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cval1)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("cval1")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cval2)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("cval2")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cval3)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("cval3")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cval4)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("cval4")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Czonag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("czonag")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("data")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Datafgctreemb)
                    .HasColumnType("datetime")
                    .HasColumnName("datafgctreemb")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Datajovemisentoirs)
                    .HasColumnType("datetime")
                    .HasColumnName("datajovemisentoirs")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Diaplano)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("diaplano");

                entity.Property(e => e.Dias)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("dias");

                entity.Property(e => e.Dilnoplano)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("dilnoplano");

                entity.Property(e => e.Dinoplano)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("dinoplano")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Docno)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("docno");

                entity.Property(e => e.Docnome)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("docnome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Dpago)
                    .HasColumnType("datetime")
                    .HasColumnName("dpago")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Dplano)
                    .HasColumnType("datetime")
                    .HasColumnName("dplano")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.DteventoSupinfSs)
                    .HasColumnType("datetime")
                    .HasColumnName("dtevento_supinf_ss")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Eacidente)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eacidente");

                entity.Property(e => e.Eadseevalor)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eadseevalor");

                entity.Property(e => e.Ebasedia)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ebasedia");

                entity.Property(e => e.Ebasemes)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ebasemes");

                entity.Property(e => e.Ebonus)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ebonus");

                entity.Property(e => e.Ecgaevalor)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ecgaevalor");

                entity.Property(e => e.Efctvalor)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("efctvalor");

                entity.Property(e => e.Efgctvalor)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("efgctvalor");

                entity.Property(e => e.Efgctvalreemb)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("efgctvalreemb");

                entity.Property(e => e.Efuncp).HasColumnName("efuncp");

                entity.Property(e => e.Ehett)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ehett");

                entity.Property(e => e.Eirsacum)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eirsacum");

                entity.Property(e => e.Eliquido)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eliquido");

                entity.Property(e => e.Escala)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("escala")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Essevalor)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("essevalor");

                entity.Property(e => e.Esshextra)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("esshextra");

                entity.Property(e => e.Estab)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("estab");

                entity.Property(e => e.Esubsidio)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("esubsidio");

                entity.Property(e => e.Ettdesc)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ettdesc");

                entity.Property(e => e.Ettnotliquido)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ettnotliquido");

                entity.Property(e => e.Ettnsuj)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ettnsuj");

                entity.Property(e => e.Ettsuj)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ettsuj");

                entity.Property(e => e.Ettsujadse)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ettsujadse");

                entity.Property(e => e.Ettsujart2b)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ettsujart2b");

                entity.Property(e => e.Ettsujcga)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ettsujcga");

                entity.Property(e => e.Ettsujcx)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ettsujcx");

                entity.Property(e => e.Evalorjaemtb)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evalorjaemtb");

                entity.Property(e => e.Evalorol)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evalorol");

                entity.Property(e => e.Evencacum)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evencacum");

                entity.Property(e => e.EventoSupinfSs)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("evento_supinf_ss")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Eviansujirs)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eviansujirs");

                entity.Property(e => e.Fctvalor)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("fctvalor");

                entity.Property(e => e.Fgctmesstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("fgctmesstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Fgctreemb).HasColumnName("fgctreemb");

                entity.Property(e => e.Fgctvalor)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("fgctvalor");

                entity.Property(e => e.Fgctvalreemb)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("fgctvalreemb");

                entity.Property(e => e.Fref)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("fref")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gpccheque)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("gpccheque")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gpcok).HasColumnName("gpcok");

                entity.Property(e => e.Gpcstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("gpcstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Grupo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("grupo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Heqt)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("heqt");

                entity.Property(e => e.Hett)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("hett");

                entity.Property(e => e.Horasextra)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("horasextra");

                entity.Property(e => e.Horasfalta)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("horasfalta");

                entity.Property(e => e.Horasmes)
                    .HasColumnType("numeric(5, 1)")
                    .HasColumnName("horasmes");

                entity.Property(e => e.Intid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("intid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Irsacum)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("irsacum");

                entity.Property(e => e.Jaajirs).HasColumnName("jaajirs");

                entity.Property(e => e.Jovemisentoirs).HasColumnName("jovemisentoirs");

                entity.Property(e => e.Liquido)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("liquido");

                entity.Property(e => e.Lol).HasColumnName("lol");

                entity.Property(e => e.Ltrab)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("ltrab")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Mesref)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("mesref")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Mliquido)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("mliquido");

                entity.Property(e => e.Moeda)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("moeda")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Mvalorjaemtb)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("mvalorjaemtb");

                entity.Property(e => e.Mvalorol)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("mvalorol");

                entity.Property(e => e.Nada).HasColumnName("nada");

                entity.Property(e => e.Nasc)
                    .HasColumnType("datetime")
                    .HasColumnName("nasc")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Nbenef)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("nbenef")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ncategoria)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ncategoria");

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

                entity.Property(e => e.Nhabil)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("nhabil");

                entity.Property(e => e.Nib)
                    .HasMaxLength(28)
                    .IsUnicode(false)
                    .HasColumnName("nib")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nivel)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("nivel")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.No)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("no");

                entity.Property(e => e.Noconta)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("noconta")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nome)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("nome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nomecomp)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("nomecomp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nosind)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("nosind")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Notrf).HasColumnName("notrf");

                entity.Property(e => e.Obsss)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("obsss")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Obsss2)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("obsss2")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Olcodigo)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("olcodigo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Oldata)
                    .HasColumnType("datetime")
                    .HasColumnName("oldata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

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

                entity.Property(e => e.Owdescricao)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("owdescricao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Pago).HasColumnName("pago");

                entity.Property(e => e.Plano).HasColumnName("plano");

                entity.Property(e => e.Prid)
                    .HasColumnType("numeric(12, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("prid");

                entity.Property(e => e.Primeiro).HasColumnName("primeiro");

                entity.Property(e => e.Recibo)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("recibo");

                entity.Property(e => e.Recno)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("recno");

                entity.Property(e => e.Resestr).HasColumnName("resestr");

                entity.Property(e => e.Seccao)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("seccao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Seguro)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("seguro")
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

                entity.Property(e => e.Sgrupo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("sgrupo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Sind)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("sind")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Sitprof)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("sitprof");

                entity.Property(e => e.Sscodigo)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("sscodigo");

                entity.Property(e => e.Ssevalor)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ssevalor");

                entity.Property(e => e.Sshextra)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("sshextra");

                entity.Property(e => e.Status)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("status");

                entity.Property(e => e.Subsidio)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("subsidio");

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

                entity.Property(e => e.Telanno)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("telanno");

                entity.Property(e => e.Telocal)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("telocal")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ttdesc)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ttdesc");

                entity.Property(e => e.Ttnotliquido)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ttnotliquido");

                entity.Property(e => e.Ttnsuj)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ttnsuj");

                entity.Property(e => e.Ttsuj)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ttsuj");

                entity.Property(e => e.Ttsujadse)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ttsujadse");

                entity.Property(e => e.Ttsujart2b)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ttsujart2b");

                entity.Property(e => e.Ttsujcga)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ttsujcga");

                entity.Property(e => e.Ttsujcx)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ttsujcx");

                entity.Property(e => e.Txirs).HasColumnName("txirs");

                entity.Property(e => e.Txsse)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("txsse");

                entity.Property(e => e.Txssp)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("txssp");

                entity.Property(e => e.UAltpr)
                    .HasColumnType("text")
                    .HasColumnName("u_altpr")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UBonproc).HasColumnName("u_bonproc");

                entity.Property(e => e.UCaixa)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("u_caixa")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UCambio)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("u_cambio");

                entity.Property(e => e.UCastsuj)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_castsuj");

                entity.Property(e => e.UCastxp)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("u_castxp");

                entity.Property(e => e.UCcusto)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_ccusto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UClsfcfm)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_clsfcfm")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDespcalc).HasColumnName("u_despcalc");

                entity.Property(e => e.UDistres)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_distres")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDta)
                    .HasColumnType("datetime")
                    .HasColumnName("u_dta")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.UEscalao)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("u_escalao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UEvent)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("u_event")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UExclinss).HasColumnName("u_exclinss");

                entity.Property(e => e.UFicheiro)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_ficheiro")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UHrecibo)
                    .HasColumnType("numeric(16, 0)")
                    .HasColumnName("u_hrecibo");

                entity.Property(e => e.UImport).HasColumnName("u_import");

                entity.Property(e => e.ULastproc).HasColumnName("u_lastproc");

                entity.Property(e => e.UMinnac)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("u_minnac");

                entity.Property(e => e.UMintrib)
                    .HasColumnType("numeric(12, 0)")
                    .HasColumnName("u_mintrib");

                entity.Property(e => e.UMtliq)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("u_mtliq");

                entity.Property(e => e.UNocas).HasColumnName("u_nocas");

                entity.Property(e => e.UNome)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("u_nome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNrantigo)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("u_nrantigo");

                entity.Property(e => e.UNrcard)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("u_nrcard")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNumproc)
                    .HasColumnType("numeric(12, 0)")
                    .HasColumnName("u_numproc");

                entity.Property(e => e.UProcpor)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("u_procpor")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.USstvalor)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_sstvalor");

                entity.Property(e => e.UTcontr)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("u_tcontr")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UTxxpe)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("u_txxpe");

                entity.Property(e => e.UUsdliq)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("u_usdliq");

                entity.Property(e => e.UValorxpe)
                    .HasColumnType("numeric(16, 5)")
                    .HasColumnName("u_valorxpe");

                entity.Property(e => e.UVlxpeatr)
                    .HasColumnType("numeric(16, 5)")
                    .HasColumnName("u_vlxpeatr");

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

                entity.Property(e => e.Valorjaemtb)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("valorjaemtb");

                entity.Property(e => e.Valorol)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("valorol");

                entity.Property(e => e.Vencacum)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("vencacum");

                entity.Property(e => e.Viansujirs)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("viansujirs");
            });


            modelBuilder.Entity<Ow>(entity =>
            {
                entity.HasKey(e => e.Owstamp)
                    .HasName("pk_ow")
                    .IsClustered(false);

                entity.ToTable("ow");

                entity.HasIndex(e => e.Data, "in_ow_data")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.Data, e.Docnome, e.Cheque, e.Descricao, e.Entr, e.Said, e.Ollocal, e.Eentr, e.Esaid, e.Owstamp }, "in_ow_owlist")
                    .HasFillFactor(80);

                entity.Property(e => e.Owstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("owstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Adoc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("adoc")
                    .HasDefaultValueSql("('')");

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

                entity.Property(e => e.Docno)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("docno");

                entity.Property(e => e.Docnome)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("docnome")
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

                entity.Property(e => e.Eentr)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eentr");

                entity.Property(e => e.Entr)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("entr");

                entity.Property(e => e.Entrm1)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("entrm1");

                entity.Property(e => e.Entrm2)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("entrm2");

                entity.Property(e => e.Esaid)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("esaid");

                entity.Property(e => e.Exportado).HasColumnName("exportado");

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

                entity.Property(e => e.Lancancont).HasColumnName("lancancont");

                entity.Property(e => e.Lancapen).HasColumnName("lancapen");

                entity.Property(e => e.Local)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("local")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Lordem)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("lordem");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Moeda1)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("moeda1")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Moeda2)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("moeda2")
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

                entity.Property(e => e.Olcodigo)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("olcodigo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ollinhas).HasColumnName("ollinhas");

                entity.Property(e => e.Ollocal)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ollocal")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Olstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("olstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Operext).HasColumnName("operext");

                entity.Property(e => e.Origem)
                    .HasMaxLength(10)
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

                entity.Property(e => e.Owid)
                    .HasColumnType("numeric(12, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("owid");

                entity.Property(e => e.Owliolcod).HasColumnName("owliolcod");

                entity.Property(e => e.Ozstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ozstamp")
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

                entity.Property(e => e.Postamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("postamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Processo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("processo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Prstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("prstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Recibo)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("recibo");

                entity.Property(e => e.Restamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("restamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Said)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("said");

                entity.Property(e => e.Saidm1)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("saidm1");

                entity.Property(e => e.Saidm2)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("saidm2");

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

                entity.Property(e => e.Subproc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("subproc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UBalcao)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("u_balcao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UBanco)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_banco")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UBenno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("u_benno");

                entity.Property(e => e.UBennome)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("u_bennome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UCaixa)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("u_caixa")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UConta)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_conta")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UContab)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_contab")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDatatrf)
                    .HasColumnType("datetime")
                    .HasColumnName("u_datatrf")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.UDebita)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_debita")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UEstabfl)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("u_estabfl");

                entity.Property(e => e.UFactor)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("u_factor")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UFdbanco)
                    .HasMaxLength(28)
                    .IsUnicode(false)
                    .HasColumnName("u_fdbanco")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UFdconta)
                    .HasMaxLength(28)
                    .IsUnicode(false)
                    .HasColumnName("u_fdconta")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UFdnib)
                    .HasMaxLength(28)
                    .IsUnicode(false)
                    .HasColumnName("u_fdnib")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UFicheiro)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_ficheiro")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UImpresso).HasColumnName("u_impresso");

                entity.Property(e => e.UIndexada).HasColumnName("u_indexada");

                entity.Property(e => e.UIntgrok).HasColumnName("u_intgrok");

                entity.Property(e => e.UMcam)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_mcam");

                entity.Property(e => e.UMoe)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("u_moe")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNfunc)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("u_nfunc");

                entity.Property(e => e.UNib)
                    .HasMaxLength(28)
                    .IsUnicode(false)
                    .HasColumnName("u_nib")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNobanco)
                    .HasMaxLength(28)
                    .IsUnicode(false)
                    .HasColumnName("u_nobanco")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNoconta)
                    .HasMaxLength(28)
                    .IsUnicode(false)
                    .HasColumnName("u_noconta")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNofl)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("u_nofl");

                entity.Property(e => e.UNome)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("u_nome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNomefl)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("u_nomefl")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNomefunc)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("u_nomefunc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNope)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("u_nope");

                entity.Property(e => e.UOwcam)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_owcam");

                entity.Property(e => e.UOwent)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_owent");

                entity.Property(e => e.UOwsaid)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_owsaid");

                entity.Property(e => e.UOwtot)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_owtot");

                entity.Property(e => e.USendpay).HasColumnName("u_sendpay");

                entity.Property(e => e.UUsrtrf)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("u_usrtrf")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UXiporo).HasColumnName("u_xiporo");

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

                entity.Property(e => e.Vrstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("vrstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();
            });


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