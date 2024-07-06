using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using LENMEDWS.Domains.Models;
using LENMEDWS.Extensions;

namespace LENMEDWS.Persistence.Contexts
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


        public virtual DbSet<Mr> Mr { get; set; } = null!;

        public virtual DbSet<E1> E1 { get; set; } = null!;
        public virtual DbSet<Cu> Cu { get; set; } = null!;
        public virtual DbSet<Do> Do { get; set; } = null!;
        public virtual DbSet<Ml> Ml { get; set; } = null!;
        public virtual DbSet<Para1> Para1 { get; set; } = null!; 
        public virtual DbSet<Log> Log { get; set; } = null!;
        public virtual DbSet<ApiLogs> ApiLogs { get; set; } = null!;
        public virtual DbSet<UProvider> UProvider { get; set; } = null!;
        public virtual DbSet<Ft> Ft { get; set; } = null!;
        public virtual DbSet<Ft3> Ft3 { get; set; } = null!;
        public virtual DbSet<Ft2> Ft2 { get; set; } = null!;

        public virtual DbSet<Us> Us { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DBconnect"));
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


            modelBuilder.Entity<Mr>(entity =>
            {
                entity.HasKey(e => e.Mrno)
                    .HasName("pk_mr")
                    .IsClustered(false);

                entity.ToTable("mr");

                entity.HasIndex(e => e.Mrno, "in_mr_mrno")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Mrstamp, "in_mr_stamp")
                    .IsUnique()
                    .HasFillFactor(70);

                entity.Property(e => e.Mrno)
                    .HasColumnType("numeric(12, 0)")
                    .HasColumnName("mrno");

                entity.Property(e => e.Agestab)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("agestab");

                entity.Property(e => e.Apelido)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("apelido")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Chefim)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("chefim")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Chehora)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("chehora")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Clestab)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("clestab");

                entity.Property(e => e.Clno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("clno");

                entity.Property(e => e.Clnome)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("clnome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Coddiv)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("coddiv");

                entity.Property(e => e.Codpresc)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("codpresc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("data")
                    .HasDefaultValueSql("(convert(datetime,'19000101'))");

                entity.Property(e => e.Diag)
                    .HasColumnType("text")
                    .HasColumnName("diag")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Div)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("div")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Drno)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("drno");

                entity.Property(e => e.Drnome)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("drnome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Entidade)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("entidade");

                entity.Property(e => e.Estado)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("estado");

                entity.Property(e => e.Etotag)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("etotag");

                entity.Property(e => e.Etotcl)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("etotcl");

                entity.Property(e => e.Etotref)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("etotref");

                entity.Property(e => e.Exam)
                    .HasColumnType("text")
                    .HasColumnName("exam")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fim)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("fim");

                entity.Property(e => e.Ftent).HasColumnName("ftent");

                entity.Property(e => e.Ftpes).HasColumnName("ftpes");

                entity.Property(e => e.Hfim)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("hfim")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Hinicio)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("hinicio")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Idno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("idno");

                entity.Property(e => e.Idnome)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("idnome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ind).HasColumnName("ind");

                entity.Property(e => e.Inicio)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("inicio");

                entity.Property(e => e.Lpresccod)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("lpresccod")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Medi)
                    .HasColumnType("text")
                    .HasColumnName("medi")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Mrfim)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("mrfim");

                entity.Property(e => e.Mrinicio)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("mrinicio");

                entity.Property(e => e.Mrpri).HasColumnName("mrpri");

                entity.Property(e => e.Mrstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("mrstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Nmentidade)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("nmentidade")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nome)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("nome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Obs)
                    .HasColumnType("text")
                    .HasColumnName("obs")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Obse)
                    .HasColumnType("text")
                    .HasColumnName("obse")
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

                entity.Property(e => e.Pediurec).HasColumnName("pediurec");

                entity.Property(e => e.Quempaga)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("quempaga");

                entity.Property(e => e.Razrejadse)
                    .HasColumnType("text")
                    .HasColumnName("razrejadse")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Rejadse).HasColumnName("rejadse");

                entity.Property(e => e.Rela)
                    .HasColumnType("text")
                    .HasColumnName("rela")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Sint)
                    .HasColumnType("text")
                    .HasColumnName("sint")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Speriodo).HasColumnName("speriodo");

                entity.Property(e => e.Telefone)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("telefone")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Telesc)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("telesc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tlmvl)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("tlmvl")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Totag)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("totag");

                entity.Property(e => e.Totcl)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("totcl");

                entity.Property(e => e.Totref)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("totref");

                entity.Property(e => e.UAlta)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("u_alta");

                entity.Property(e => e.UAntes12)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("u_antes12");

                entity.Property(e => e.UAutorimb)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_autorimb")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UAutorinm)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("u_autorinm")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UAutorino)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("u_autorino")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UAutoriz).HasColumnName("u_autoriz");

                entity.Property(e => e.UCama)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("u_cama")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UCambio)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_cambio");

                entity.Property(e => e.UCateter)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("u_cateter");

                entity.Property(e => e.UCaucao)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_caucao");

                entity.Property(e => e.UCid10)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("u_cid10")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UClinica)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_clinica")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UCompno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("u_compno")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDalta)
                    .HasColumnType("datetime")
                    .HasColumnName("u_dalta")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.UDatam)
                    .HasColumnType("datetime")
                    .HasColumnName("u_datam")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.UDescmot)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_descmot")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDias)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("u_dias");

                entity.Property(e => e.UDieta)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("u_dieta")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDmorte)
                    .HasColumnType("datetime")
                    .HasColumnName("u_dmorte")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.UDoen1)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("u_doen1");

                entity.Property(e => e.UDoen2)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("u_doen2");

                entity.Property(e => e.UDoen3)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("u_doen3");

                entity.Property(e => e.UDoencap)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("u_doencap")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDoencas1)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("u_doencas1")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDoencas2)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("u_doencas2")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDoencas3)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("u_doencas3")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDprincip)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("u_dprincip");

                entity.Property(e => e.UDtver)
                    .HasColumnType("datetime")
                    .HasColumnName("u_dtver")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.UEnferma)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_enferma")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UEspec)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_espec")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UEspolio).HasColumnName("u_espolio");

                entity.Property(e => e.UEstab)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_estab")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UExameobj)
                    .HasColumnType("text")
                    .HasColumnName("u_exameobj")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UExamesub)
                    .HasColumnType("text")
                    .HasColumnName("u_examesub")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UGrau)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("u_grau")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UHalta)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("u_halta")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UHfalec)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("u_hfalec")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UHmorte)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("u_hmorte")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UHoram)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("u_horam")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UIcirurg).HasColumnName("u_icirurg");

                entity.Property(e => e.UIdespec)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("u_idespec");

                entity.Property(e => e.UIsentcau).HasColumnName("u_isentcau");

                entity.Property(e => e.ULoctrab)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("u_loctrab")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UMalta)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_malta")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UMedicc)
                    .HasColumnType("text")
                    .HasColumnName("u_medicc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UMedico)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_medico")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UMole).HasColumnName("u_mole");

                entity.Property(e => e.UMorada)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("u_morada")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UMotadm)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("u_motadm");

                entity.Property(e => e.UNome)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_nome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UObitdata)
                    .HasColumnType("datetime")
                    .HasColumnName("u_obitdata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.UObithora)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("u_obithora")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UObs)
                    .HasColumnType("text")
                    .HasColumnName("u_obs")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UPlano)
                    .HasColumnType("text")
                    .HasColumnName("u_plano")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UProcess)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("u_process");

                entity.Property(e => e.URdoen1)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("u_rdoen1");

                entity.Property(e => e.URdoen2)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("u_rdoen2");

                entity.Property(e => e.URdoen3)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("u_rdoen3");

                entity.Property(e => e.URglobal)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("u_rglobal");

                entity.Property(e => e.URprincip)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("u_rprincip");

                entity.Property(e => e.UTelef)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("u_telef")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UTeltrab)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("u_teltrab")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UTipoadm)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("u_tipoadm");

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


            modelBuilder.Entity<E1>(entity =>
            {
                entity.HasKey(e => e.Estab)
                    .HasName("pk_e1")
                    .IsClustered(false);

                entity.ToTable("e1");

                entity.HasIndex(e => new { e.Nomecomp, e.Morada, e.Estab, e.E1stamp }, "in_e1_e1list")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Estab, "in_e1_estab")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.E1stamp, "in_e1_stamp")
                    .IsUnique()
                    .HasFillFactor(70);

                entity.Property(e => e.Estab)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("estab");

                entity.Property(e => e.Actoutra)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("actoutra")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Actprinc)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("actprinc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Anoconst)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("anoconst");

                entity.Property(e => e.Asspatr)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("asspatr")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Balcao)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("balcao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Banco)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("banco")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Bcod)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("bcod")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Bfiscal)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("bfiscal");

                entity.Property(e => e.Bic)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("bic")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Blbanco)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("blbanco")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca2h1f)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca2h1f")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca2h1i)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca2h1i")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca2h2f)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca2h2f")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca2h2i)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca2h2i")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca3h1f)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca3h1f")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca3h1i)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca3h1i")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca3h2f)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca3h2f")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca3h2i)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca3h2i")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca4h1f)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca4h1f")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca4h1i)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca4h1i")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca4h2f)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca4h2f")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca4h2i)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca4h2i")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca5h1f)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca5h1f")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca5h1i)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca5h1i")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca5h2f)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca5h2f")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca5h2i)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca5h2i")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca6h1f)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca6h1f")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca6h1i)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca6h1i")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca6h2f)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca6h2f")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca6h2i)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca6h2i")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca7h1f)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca7h1f")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca7h1i)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca7h1i")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca7h2f)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca7h2f")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca7h2i)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca7h2i")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca8h1f)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca8h1f")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca8h1i)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca8h1i")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca8h2f)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca8h2f")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ca8h2i)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ca8h2i")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cae)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cae")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Capsocial)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("capsocial");

                entity.Property(e => e.Ccusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ccusto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cgaespecial).HasColumnName("cgaespecial");

                entity.Property(e => e.Codasspatr)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("codasspatr");

                entity.Property(e => e.Codconc)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("codconc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Codcrss)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("codcrss");

                entity.Property(e => e.Coddist)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("coddist")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Codentcgd)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("codentcgd")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Codfreg)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("codfreg")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Codnatjur)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("codnatjur");

                entity.Property(e => e.Codpais)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("codpais")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Codpost)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("codpost")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Concelho)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("concelho")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Consreg)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("consreg")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Contacto)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("contacto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Contado)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("contado");

                entity.Property(e => e.Crepfin)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("crepfin");

                entity.Property(e => e.Ctaccarg)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ctaccarg")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ctacttel)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("ctacttel")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Dataconst)
                    .HasColumnType("datetime")
                    .HasColumnName("dataconst")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Datafecho)
                    .HasColumnType("datetime")
                    .HasColumnName("datafecho")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Distrito)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("distrito")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Dprgremun)
                    .HasColumnType("text")
                    .HasColumnName("dprgremun")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Dprgvaldia)
                    .HasColumnType("text")
                    .HasColumnName("dprgvaldia")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.E1stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("e1stamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Ecapsocial)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ecapsocial");

                entity.Property(e => e.Efundsal)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("efundsal");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Emp1)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("emp1")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Emp2)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("emp2")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Emp3)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("emp3")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Emp4)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("emp4")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Emp5)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("emp5")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Emp6)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("emp6")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Emp7)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("emp7")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Emp8)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("emp8")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empcon1)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("empcon1")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empcon2)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("empcon2")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empcon3)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("empcon3")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empcon4)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("empcon4")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empcon5)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("empcon5")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empcon6)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("empcon6")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empcon7)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("empcon7")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empcon8)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("empcon8")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empnc1)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("empnc1")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empnc2)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("empnc2")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empnc3)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("empnc3")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empnc4)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("empnc4")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empnc5)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("empnc5")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empnc6)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("empnc6")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empnc7)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("empnc7")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Empnc8)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("empnc8")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Epensocialnctr)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("epensocialnctr");

                entity.Property(e => e.Eretrmin)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eretrmin");

                entity.Property(e => e.Esede).HasColumnName("esede");

                entity.Property(e => e.Estabid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("estabid");

                entity.Property(e => e.Estabss)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("estabss");

                entity.Property(e => e.Estaco)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("estaco");

                entity.Property(e => e.Estcont)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("estcont");

                entity.Property(e => e.Estestr)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("estestr");

                entity.Property(e => e.Estmad)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("estmad");

                entity.Property(e => e.Evolvendas)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evolvendas");

                entity.Property(e => e.Fax)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("fax")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Faxind)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("faxind")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Folhas)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("folhas")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Formgest)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("formgest")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fref)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("fref")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Freguesia)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("freguesia")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fundsal)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("fundsal");

                entity.Property(e => e.Ger1)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ger1")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ger2)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ger2")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ger3)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ger3")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ger4)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ger4")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ger5)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ger5")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ger6)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ger6")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ger7)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ger7")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ger8)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ger8")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ger9)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ger9")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gerc1)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("gerc1")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gerc2)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("gerc2")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gerc3)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("gerc3")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gerc4)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("gerc4")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gerc5)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("gerc5")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gerc6)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("gerc6")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gerc7)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("gerc7")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gerc8)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("gerc8")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gerc9)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("gerc9")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gernco1)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("gernco1")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gernco2)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("gernco2")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gernco3)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("gernco3")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gernco4)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("gernco4")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gernco5)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("gernco5")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gernco6)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("gernco6")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gernco7)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("gernco7")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gernco8)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("gernco8")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gernco9)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("gernco9")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Grpci)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("grpci")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Grupo)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("grupo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Iban)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("iban")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Idcredor)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("idcredor");

                entity.Property(e => e.Idcredorsepa)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("idcredorsepa")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Idulupd).HasColumnName("idulupd");

                entity.Property(e => e.Instss)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("instss")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Livro)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("livro")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Locact)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("locact");

                entity.Property(e => e.Local)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("local")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Locanexo1)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("locanexo1");

                entity.Property(e => e.Locanexo2)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("locanexo2");

                entity.Property(e => e.Locsede)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("locsede")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Lpresccod)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("lpresccod")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Missao)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("missao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Morada)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("morada")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Morand)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("morand")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Mornum)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("mornum")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nadseserv)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("nadseserv");

                entity.Property(e => e.Natjurid)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("natjurid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ncgaserv)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("ncgaserv");

                entity.Property(e => e.Ncont)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ncont")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nconta)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("nconta")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ncontss)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("ncontss")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ncusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ncusto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nef)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("nef")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nib)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("nib")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Niec)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("niec")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nifrl)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("nifrl")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nifroc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("nifroc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Niftoc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("niftoc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nipc)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("nipc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nipcadse)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("nipcadse")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nipccga)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("nipccga")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nipcss)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("nipcss")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nomabrv)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("nomabrv")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nomatri)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("nomatri")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nomecomp)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("nomecomp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nomeef)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("nomeef")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Normacont)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("normacont");

                entity.Property(e => e.Numclicgd)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("numclicgd")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Orgtut)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("orgtut")
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

                entity.Property(e => e.Pais)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("pais")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Partep)
                    .HasColumnType("numeric(5, 1)")
                    .HasColumnName("partep");

                entity.Property(e => e.Partest)
                    .HasColumnType("numeric(5, 1)")
                    .HasColumnName("partest");

                entity.Property(e => e.Partestr)
                    .HasColumnType("numeric(5, 1)")
                    .HasColumnName("partestr");

                entity.Property(e => e.Partout)
                    .HasColumnType("numeric(5, 1)")
                    .HasColumnName("partout");

                entity.Property(e => e.Partpriv)
                    .HasColumnType("numeric(5, 1)")
                    .HasColumnName("partpriv");

                entity.Property(e => e.Pensocialnctr)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("pensocialnctr");

                entity.Property(e => e.Percadse)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("percadse");

                entity.Property(e => e.Perccga)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("perccga");

                entity.Property(e => e.Perd1)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perd1");

                entity.Property(e => e.Perd2)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perd2");

                entity.Property(e => e.Perd3)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perd3");

                entity.Property(e => e.Perd4)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perd4");

                entity.Property(e => e.Perd5)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perd5");

                entity.Property(e => e.Perd6)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perd6");

                entity.Property(e => e.Perd7)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perd7");

                entity.Property(e => e.Perd8)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perd8");

                entity.Property(e => e.Perp1)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perp1");

                entity.Property(e => e.Perp2)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perp2");

                entity.Property(e => e.Perp3)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perp3");

                entity.Property(e => e.Perp4)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perp4");

                entity.Property(e => e.Perp5)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perp5");

                entity.Property(e => e.Perp6)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perp6");

                entity.Property(e => e.Perp7)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perp7");

                entity.Property(e => e.Perp8)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("perp8");

                entity.Property(e => e.Prgeuro).HasColumnName("prgeuro");

                entity.Property(e => e.Prgeurovaldia).HasColumnName("prgeurovaldia");

                entity.Property(e => e.Prgremun)
                    .HasColumnType("text")
                    .HasColumnName("prgremun")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Prgvaldia)
                    .HasColumnType("text")
                    .HasColumnName("prgvaldia")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Qpappver)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("qpappver")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Qpemail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("qpemail")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Qpfax)
                    .HasColumnType("numeric(9, 0)")
                    .HasColumnName("qpfax");

                entity.Property(e => e.Qpnmapp)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("qpnmapp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Qpnmep)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("qpnmep")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Qpnome)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("qpnome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Qptel1)
                    .HasColumnType("numeric(9, 0)")
                    .HasColumnName("qptel1");

                entity.Property(e => e.Qptel2)
                    .HasColumnType("numeric(9, 0)")
                    .HasColumnName("qptel2");

                entity.Property(e => e.Repfin)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("repfin")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Retrmin)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("retrmin");

                entity.Property(e => e.Rzsoc)
                    .HasMaxLength(66)
                    .IsUnicode(false)
                    .HasColumnName("rzsoc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Sedeid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("sedeid");

                entity.Property(e => e.Slogan)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("slogan")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Telefone)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("telefone")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Teleind)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("teleind")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Telex)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("telex")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tiporesid)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("tiporesid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDecompbe).HasColumnName("u_decompbe");

                entity.Property(e => e.UDotgrav).HasColumnName("u_dotgrav");

                entity.Property(e => e.UImobst).HasColumnName("u_imobst");

                entity.Property(e => e.UImobx64)
                    .HasMaxLength(254)
                    .IsUnicode(false)
                    .HasColumnName("u_imobx64")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ULarrprde).HasColumnName("u_larrprde");

                entity.Property(e => e.ULsissmo).HasColumnName("u_lsissmo");

                entity.Property(e => e.UNobeauto).HasColumnName("u_nobeauto");

                entity.Property(e => e.UPath)
                    .HasColumnType("text")
                    .HasColumnName("u_path")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UPath2)
                    .HasColumnType("text")
                    .HasColumnName("u_path2")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UPath3)
                    .HasColumnType("text")
                    .HasColumnName("u_path3")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UPath4)
                    .HasColumnType("text")
                    .HasColumnName("u_path4")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UTemp)
                    .HasColumnType("text")
                    .HasColumnName("u_temp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ubigeo)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("ubigeo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Url)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("url")
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

                entity.Property(e => e.Visao)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("visao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Volneg)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("volneg");

                entity.Property(e => e.Volvendas)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("volvendas");
            });


            modelBuilder.Entity<Cu>(entity =>
            {
                entity.HasKey(e => e.Cct)
                    .HasName("pk_cu")
                    .IsClustered(false);

                entity.ToTable("cu");

                entity.HasIndex(e => new { e.Descricao, e.Cct, e.Custamp }, "in_cu_culist")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Descricao, "in_cu_desc")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Custamp, "in_cu_stamp")
                    .IsUnique()
                    .HasFillFactor(70);

                entity.Property(e => e.Cct)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cct")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Custamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("custamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("descricao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");

                entity.Property(e => e.Integracao).HasColumnName("integracao");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

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


            modelBuilder.Entity<Do>(entity =>
            {
                entity.HasKey(e => e.Dostamp)
                    .HasName("pk_do")
                    .IsClustered(false);

                entity.ToTable("do");

                entity.HasIndex(e => new { e.Data, e.Mes }, "in_do_data")
                    .HasFillFactor(70);

                entity.HasIndex(e => new { e.Dino, e.Dilno, e.Ano }, "in_do_dilno")
                    .IsUnique()
                    .HasFillFactor(70);

                entity.HasIndex(e => new { e.Dino, e.Dinome, e.Dilno, e.Docnome, e.Adoc, e.Data, e.Mes, e.Dostamp }, "in_do_dolist")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Dostamp, "in_do_stamp")
                    .IsUnique()
                    .HasFillFactor(70);

                entity.Property(e => e.Dostamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("dostamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Adoc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("adoc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Anexo41)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("anexo41")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ano)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("ano");

                entity.Property(e => e.Apstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("apstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Basees)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("basees");

                entity.Property(e => e.Basesp)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("basesp");

                entity.Property(e => e.Cambio)
                    .HasColumnType("numeric(16, 6)")
                    .HasColumnName("cambio");

                entity.Property(e => e.Conf1).HasColumnName("conf1");

                entity.Property(e => e.Conf2).HasColumnName("conf2");

                entity.Property(e => e.Creana)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("creana");

                entity.Property(e => e.Crefin)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("crefin");

                entity.Property(e => e.Creord)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("creord");

                entity.Property(e => e.Criadoimp).HasColumnName("criadoimp");

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("data")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Datadiva)
                    .HasColumnType("datetime")
                    .HasColumnName("datadiva")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Datarect)
                    .HasColumnType("datetime")
                    .HasColumnName("datarect")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Debana)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("debana");

                entity.Property(e => e.Debfin)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("debfin");

                entity.Property(e => e.Debord)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("debord");

                entity.Property(e => e.Dia)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("dia");

                entity.Property(e => e.Dilno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("dilno");

                entity.Property(e => e.Dilnoid)
                    .HasColumnType("numeric(14, 0)")
                    .HasColumnName("dilnoid");

                entity.Property(e => e.Dino)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("dino");

                entity.Property(e => e.Dinome)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("dinome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Docdata)
                    .HasColumnType("datetime")
                    .HasColumnName("docdata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Docnome)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("docnome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Doctipo)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("doctipo");

                entity.Property(e => e.Ebasees)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ebasees");

                entity.Property(e => e.Ebasesp)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ebasesp");

                entity.Property(e => e.Ecreana)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ecreana");

                entity.Property(e => e.Ecrefin)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ecrefin");

                entity.Property(e => e.Ecreord)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ecreord");

                entity.Property(e => e.Edebana)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("edebana");

                entity.Property(e => e.Edebfin)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("edebfin");

                entity.Property(e => e.Edebord)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("edebord");

                entity.Property(e => e.Einvsuj).HasColumnName("einvsuj");

                entity.Property(e => e.Eivaes)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivaes");

                entity.Property(e => e.Eivasp)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivasp");

                entity.Property(e => e.Identdecexp)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("identdecexp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ivaes)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivaes");

                entity.Property(e => e.Ivasp)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivasp");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Meiost).HasColumnName("meiost");

                entity.Property(e => e.Memissao)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("memissao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Mes)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("mes");

                entity.Property(e => e.Ncont)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ncont")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nomeop)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("nomeop")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Npedido)
                    .HasColumnType("numeric(13, 0)")
                    .HasColumnName("npedido");

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

                entity.Property(e => e.Pncont)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("pncont")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Prov).HasColumnName("prov");

                entity.Property(e => e.Rectranimov).HasColumnName("rectranimov");

                entity.Property(e => e.Saldoinicial).HasColumnName("saldoinicial");

                entity.Property(e => e.Strqrcode)
                    .HasColumnType("text")
                    .HasColumnName("strqrcode")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tabori)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("tabori")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tipoop)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("tipoop")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tiporeg)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tiporeg")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UltApuraCev).HasColumnName("ult_apura_cev");

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


            modelBuilder.Entity<Ml>(entity =>
            {
                entity.HasKey(e => e.Mlstamp)
                    .HasName("pk_ml")
                    .IsClustered(false);

                entity.ToTable("ml");

                entity.HasIndex(e => e.Bastamp, "in_ml_bastamp")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Cct, "in_ml_cct")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Conta, "in_ml_conta")
                    .HasFillFactor(70);

                entity.HasIndex(e => new { e.Conta, e.Mes, e.Data }, "in_ml_contadata")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Data, "in_ml_data")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Dostamp, "in_ml_dostamp")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Lordem, "in_ml_lordem")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Mes, "in_ml_mes")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Ncont, "in_ml_ncont")
                    .HasFillFactor(70);

                entity.HasIndex(e => new { e.Origem, e.Oristamp }, "in_ml_origem_oristamp")
                    .HasFillFactor(70);

                entity.Property(e => e.Mlstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("mlstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Adoc)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("adoc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Basereg).HasColumnName("basereg");

                entity.Property(e => e.Bastamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("bastamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Cambio)
                    .HasColumnType("numeric(16, 6)")
                    .HasColumnName("cambio");

                entity.Property(e => e.Cct)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cct")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cecope)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("cecope")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Chave)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("chave")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Codis)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("codis")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Codisconf)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("codisconf")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Codprovincia)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("codprovincia")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Conf1).HasColumnName("conf1");

                entity.Property(e => e.Conf2).HasColumnName("conf2");

                entity.Property(e => e.Conta)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("conta")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cre)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("cre");

                entity.Property(e => e.Crem)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("crem");

                entity.Property(e => e.Czonag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("czonag")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("data")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Deb)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("deb");

                entity.Property(e => e.Debl).HasColumnName("debl");

                entity.Property(e => e.Debm)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("debm");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("descricao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Descritivo)
                    .HasMaxLength(135)
                    .IsUnicode(false)
                    .HasColumnName("descritivo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Dia)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("dia");

                entity.Property(e => e.Dilno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("dilno");

                entity.Property(e => e.Dino)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("dino");

                entity.Property(e => e.Dinome)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("dinome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Docno)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("docno");

                entity.Property(e => e.Docnome)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("docnome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Doctipo)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("doctipo");

                entity.Property(e => e.Dostamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("dostamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Ecre)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ecre");

                entity.Property(e => e.Edeb)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("edeb");

                entity.Property(e => e.Erecapval)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("erecapval");

                entity.Property(e => e.Extracto)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("extracto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Grupo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("grupo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Idorigem)
                    .HasColumnType("numeric(12, 0)")
                    .HasColumnName("idorigem");

                entity.Property(e => e.Intid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("intid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Iva)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("iva");

                entity.Property(e => e.Ivareg).HasColumnName("ivareg");

                entity.Property(e => e.Lordem)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("lordem");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Mes)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("mes");

                entity.Property(e => e.Modalidade)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("modalidade")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ncont)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ncont")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ndeciva).HasColumnName("ndeciva");

                entity.Property(e => e.Npt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("npt")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Numcontrepres)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("numcontrepres")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Obs)
                    .HasColumnType("text")
                    .HasColumnName("obs")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Olcodigo)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("olcodigo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Operext).HasColumnName("operext");

                entity.Property(e => e.Ordem)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ordem");

                entity.Property(e => e.Origem)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("origem")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Oriinventario)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("oriinventario");

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

                entity.Property(e => e.Paistamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("paistamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Pncont)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("pncont")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Recapit)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("recapit")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Recapval)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("recapval");

                entity.Property(e => e.Reco).HasColumnName("reco");

                entity.Property(e => e.Reg).HasColumnName("reg");

                entity.Property(e => e.Rubrica)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("rubrica")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Separa).HasColumnName("separa");

                entity.Property(e => e.Sgrupo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("sgrupo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tabiva)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("tabiva");

                entity.Property(e => e.Tipoiva)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("tipoiva")
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

                entity.Property(e => e.Vemdedc).HasColumnName("vemdedc");

                entity.Property(e => e.Vemdoext).HasColumnName("vemdoext");
            });

            modelBuilder.Entity<Para1>(entity =>
            {
                entity.HasKey(e => e.Descricao)
                    .HasName("pk_para1")
                    .IsClustered(false);

                entity.ToTable("para1");

                entity.HasIndex(e => e.Descricao, "in_para1_descricao")
                    .HasFillFactor(70);

                entity.HasIndex(e => new { e.Tipo, e.Valor, e.Dec, e.Tam, e.Descricao }, "in_para1_para1")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Para1stamp, "in_para1_stamp")
                    .IsUnique()
                    .HasFillFactor(70);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("descricao")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Acesso)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("acesso")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Actanexos).HasColumnName("actanexos");

                entity.Property(e => e.Apenasecran).HasColumnName("apenasecran");

                entity.Property(e => e.Conf)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("conf");

                entity.Property(e => e.Dec)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("dec");

                entity.Property(e => e.Ecran)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("ecran")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ecrannm)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("ecrannm")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Imgvalor)
                    .HasColumnType("image")
                    .HasColumnName("imgvalor")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Memvalor)
                    .HasColumnType("text")
                    .HasColumnName("memvalor")
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

                entity.Property(e => e.Para1stamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("para1stamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Pfcod)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("pfcod");

                entity.Property(e => e.Pfnm)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pfnm")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tabela)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tabela")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tacesso)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("tacesso");

                entity.Property(e => e.Tam)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("tam");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("tipo")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Usernm)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("usernm")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Userno)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("userno");

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
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("valor")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();
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

                entity.Property(e => e.UKobosync).HasColumnName("u_kobosync");

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


            modelBuilder.Entity<Us>(entity =>
            {
                entity.HasKey(e => e.Userno)
                    .HasName("pk_us")
                    .IsClustered(false);

                entity.ToTable("us");

                entity.HasIndex(e => e.Usstamp, "in_us_stamp")
                    .IsUnique()
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Usercode, "in_us_usercode")
                    .IsUnique()
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Username, "in_us_username")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.Userno, "in_us_userno")
                    .HasFillFactor(80);

                entity.Property(e => e.Userno)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("userno");

                entity.Property(e => e.Abrecalfis).HasColumnName("abrecalfis");

                entity.Property(e => e.Abremonrelcred).HasColumnName("abremonrelcred");

                entity.Property(e => e.Actk2fa)
                    .HasMaxLength(254)
                    .IsUnicode(false)
                    .HasColumnName("actk2fa")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Admdcont).HasColumnName("admdcont");

                entity.Property(e => e.Admdpess).HasColumnName("admdpess");

                entity.Property(e => e.Aextpw)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("aextpw")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Aextra).HasColumnName("aextra");

                entity.Property(e => e.Agencalfis).HasColumnName("agencalfis");

                entity.Property(e => e.Alertsweb).HasColumnName("alertsweb");

                entity.Property(e => e.Antf).HasColumnName("antf");

                entity.Property(e => e.Area)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("area")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Asusername)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("asusername")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Asuserno)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("asuserno");

                entity.Property(e => e.Autposmv).HasColumnName("autposmv");

                entity.Property(e => e.Avstamp)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("avstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Avstimer)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("avstimer")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Bwscodigo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("bwscodigo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Bwspw)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("bwspw")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Centrolog)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("centrolog")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Checkhelpt).HasColumnName("checkhelpt");

                entity.Property(e => e.Clbadm).HasColumnName("clbadm");

                entity.Property(e => e.Clbno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("clbno");

                entity.Property(e => e.Clbnome)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("clbnome")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Clbstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("clbstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cliadm).HasColumnName("cliadm");

                entity.Property(e => e.Cvenome)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("cvenome")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Cvestamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cvestamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Dataultpass)
                    .HasColumnType("datetime")
                    .HasColumnName("dataultpass")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Dexp2fa)
                    .HasColumnType("datetime")
                    .HasColumnName("dexp2fa")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Dexptk2fa)
                    .HasColumnType("datetime")
                    .HasColumnName("dexptk2fa")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Diasani).HasColumnName("diasani");

                entity.Property(e => e.Diascalfis)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("diascalfis");

                entity.Property(e => e.Diascon).HasColumnName("diascon");

                entity.Property(e => e.Dntf)
                    .HasColumnType("datetime")
                    .HasColumnName("dntf")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Dpt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("dpt")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Drno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("drno");

                entity.Property(e => e.Drnome)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("drnome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Drstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("drstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Dultrs)
                    .HasColumnType("datetime")
                    .HasColumnName("dultrs")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Emaxposmv)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("emaxposmv");

                entity.Property(e => e.Empregado)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("empregado")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Esa).HasColumnName("esa");

                entity.Property(e => e.Estatuto)
                    .HasColumnType("text")
                    .HasColumnName("estatuto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Etiquetascodigo)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("etiquetascodigo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Etiquetasdescricao)
                    .HasColumnType("text")
                    .HasColumnName("etiquetasdescricao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Exchangepass)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("exchangepass")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fazfxmanutencao).HasColumnName("fazfxmanutencao");

                entity.Property(e => e.Filtrocts)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("filtrocts")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Filtroctsstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("filtroctsstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Filtroem)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("filtroem")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Filtroemstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("filtroemstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Filtromx)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("filtromx")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Filtromxstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("filtromxstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Filtrotta)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("filtrotta")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Filtrottastamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("filtrottastamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Filtrovi)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("filtrovi")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Filtrovistamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("filtrovistamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fntf)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("fntf");

                entity.Property(e => e.Forgotdate)
                    .HasColumnType("datetime")
                    .HasColumnName("forgotdate")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.Forgotid)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("forgotid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gestordenuncias).HasColumnName("gestordenuncias");

                entity.Property(e => e.Grupo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("grupo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Hntf)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("hntf")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Homeus)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("homeus")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Hultrs)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("hultrs")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Idioma)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("idioma")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Idiomakey)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("idiomakey")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Imagem)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("imagem")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Inactivo).HasColumnName("inactivo");

                entity.Property(e => e.Iniciais)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("iniciais")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Inirs).HasColumnName("inirs");

                entity.Property(e => e.Jaidirecto).HasColumnName("jaidirecto");

                entity.Property(e => e.Jaini).HasColumnName("jaini");

                entity.Property(e => e.Loginerrado)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("loginerrado");

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Maxposmv)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("maxposmv");

                entity.Property(e => e.Mcdata)
                    .HasColumnType("datetime")
                    .HasColumnName("mcdata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Mcmes)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("mcmes");

                entity.Property(e => e.Menuesquerda).HasColumnName("menuesquerda");

                entity.Property(e => e.Nivelaprovacao)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("nivelaprovacao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Notifypw)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("notifypw")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Notifytk)
                    .HasMaxLength(254)
                    .IsUnicode(false)
                    .HasColumnName("notifytk")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Notifyus)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("notifyus")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ntfma).HasColumnName("ntfma");

                entity.Property(e => e.Nusamntlb).HasColumnName("nusamntlb");

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

                entity.Property(e => e.Pederelcred).HasColumnName("pederelcred");

                entity.Property(e => e.Peno)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("peno");

                entity.Property(e => e.Pestamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("pestamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Pntf).HasColumnName("pntf");

                entity.Property(e => e.Profission)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("profission")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Pwautent)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("pwautent")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Pwpos)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("pwpos")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Rgpdadm).HasColumnName("rgpdadm");

                entity.Property(e => e.Setpasswd).HasColumnName("setpasswd");

                entity.Property(e => e.Setpasswdintra).HasColumnName("setpasswdintra");

                entity.Property(e => e.Sgqadm).HasColumnName("sgqadm");

                entity.Property(e => e.Skypeid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("skypeid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Smsemail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("smsemail")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Susername)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("susername")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Suserno)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("suserno");

                entity.Property(e => e.Synactcem).HasColumnName("synactcem");

                entity.Property(e => e.Synactcvi).HasColumnName("synactcvi");

                entity.Property(e => e.Syncactcts).HasColumnName("syncactcts");

                entity.Property(e => e.Syncactmx).HasColumnName("syncactmx");

                entity.Property(e => e.Syncacttda).HasColumnName("syncacttda");

                entity.Property(e => e.Synccts).HasColumnName("synccts");

                entity.Property(e => e.Syncem).HasColumnName("syncem");

                entity.Property(e => e.Syncimpnovatda).HasColumnName("syncimpnovatda");

                entity.Property(e => e.Syncimpnovatta).HasColumnName("syncimpnovatta");

                entity.Property(e => e.Syncmx).HasColumnName("syncmx");

                entity.Property(e => e.Synctda).HasColumnName("synctda");

                entity.Property(e => e.Synctta).HasColumnName("synctta");

                entity.Property(e => e.Syncvi).HasColumnName("syncvi");

                entity.Property(e => e.Tecnico)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("tecnico");

                entity.Property(e => e.Tecnnm)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("tecnnm")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tema)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("tema")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tembws).HasColumnName("tembws");

                entity.Property(e => e.Temveriprstock).HasColumnName("temveriprstock");

                entity.Property(e => e.Tipoacd)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("tipoacd");

                entity.Property(e => e.Tipoacdvs)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("tipoacdvs");

                entity.Property(e => e.Tlmvl)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("tlmvl")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tntf)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("tntf");

                entity.Property(e => e.UAssin)
                    .HasColumnType("text")
                    .HasColumnName("u_assin")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UBztpass)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_bztpass")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UCancela)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("u_cancela");

                entity.Property(e => e.UCct)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("u_cct")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UCctst)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("u_cctst")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UCcusto)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("u_ccusto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UClient1)
                    .HasMaxLength(253)
                    .IsUnicode(false)
                    .HasColumnName("u_client1")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UContrat).HasColumnName("u_contrat");

                entity.Property(e => e.UCu)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_cu")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNaoper)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("u_naoper")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNapag)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("u_napag")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNapess)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("u_napess")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNapessmv)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("u_napessmv")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNaproj)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("u_naproj")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNaprov)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("u_naprov")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNcontaoc).HasColumnName("u_ncontaoc");

                entity.Property(e => e.UNo)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("u_no");

                entity.Property(e => e.UNome)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("u_nome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UResponsa).HasColumnName("u_responsa");

                entity.Property(e => e.USuperus).HasColumnName("u_superus");

                entity.Property(e => e.UTerminal)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_terminal")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UVmax)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_vmax");

                entity.Property(e => e.Ugstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ugstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Usaarea).HasColumnName("usaarea");

                entity.Property(e => e.Usatimezone).HasColumnName("usatimezone");

                entity.Property(e => e.Usavanc).HasColumnName("usavanc");

                entity.Property(e => e.Use2fa).HasColumnName("use2fa");

                entity.Property(e => e.Usercode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("usercode")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Username)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("username")
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

                entity.Property(e => e.Usstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("usstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Utcbrowser)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("utcbrowser");

                entity.Property(e => e.Utcdisplayname)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("utcdisplayname")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Utcuserid)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("utcuserid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Vendedor)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("vendedor");

                entity.Property(e => e.Vendnm)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("vendnm")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Verificachamadas).HasColumnName("verificachamadas");

                entity.Property(e => e.Vsstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("vsstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();
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
                entity.HasKey(e => e.Logstamp)
                    .HasName("PK__u_logs__9803C30F60140409");

                entity.ToTable("u_logs");

                entity.Property(e => e.Logstamp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_logsstamp");

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Content)
                    .IsUnicode(false)
                    .HasColumnName("content");

                entity.Property(e => e.ResponseText)
                    .IsUnicode(false)
                    .HasColumnName("responsetext");

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


            modelBuilder.Entity<Ft>(entity =>
            {
                entity.HasKey(e => new { e.Ndoc, e.Fno, e.Ftano })
                    .HasName("pk_ft")
                    .IsClustered(false);

                entity.ToTable("ft");

                entity.HasIndex(e => e.Anulado, "in_ft_anulado")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Arstamp, "in_ft_arstamp")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Cxstamp, "in_ft_cxstamp")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Dostamp, "in_ft_dostamp")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Estab, "in_ft_estab")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Fdata, "in_ft_fdata")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Nome, "in_ft_ftlist")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Lpstamp, "in_ft_lpstamp")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Lrstamp, "in_ft_lrstamp")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Mhstamp, "in_ft_mhstamp")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Mrstamp, "in_ft_mrstamp")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.No, "in_ft_no")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Nome, "in_ft_nome")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Rpclstamp, "in_ft_rpclstamp")
                    .HasFillFactor(70);

                entity.HasIndex(e => new { e.Site, e.Pnome, e.Cxstamp, e.Anulado, e.Fno, e.Tipodoc }, "in_ft_sangrias")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Snstamp, "in_ft_snstamp")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Ssstamp, "in_ft_ssstamp")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Ftstamp, "in_ft_stamp")
                    .IsUnique()
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Tipodoc, "in_ft_tipo_anula_ft_ndoc")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Tipodoc, "in_ft_tipodoc")
                    .HasFillFactor(70);

                entity.HasIndex(e => e.Tpstamp, "in_ft_tpstamp")
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

                entity.Property(e => e.Aprovado).HasColumnName("aprovado");

                entity.Property(e => e.Arno)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("arno");

                entity.Property(e => e.Arstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("arstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Bidata)
                    .HasColumnType("datetime")
                    .HasColumnName("bidata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Bilocal)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("bilocal")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Bino)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("bino")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cambio)
                    .HasColumnType("numeric(20, 12)")
                    .HasColumnName("cambio");

                entity.Property(e => e.Cambiofixo).HasColumnName("cambiofixo");

                entity.Property(e => e.Carga)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("carga")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ccusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ccusto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cdata)
                    .HasColumnType("datetime")
                    .HasColumnName("cdata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Chdata)
                    .HasColumnType("datetime")
                    .HasColumnName("chdata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Cheque).HasColumnName("cheque");

                entity.Property(e => e.Chmoeda)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("chmoeda")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Chora)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("chora")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Chtmoeda)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("chtmoeda");

                entity.Property(e => e.Chtotal)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("chtotal");

                entity.Property(e => e.Classe)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("classe")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Clbanco)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("clbanco")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Clcheque)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("clcheque")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cobrado).HasColumnName("cobrado");

                entity.Property(e => e.Cobrador)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cobrador")
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

                entity.Property(e => e.Custo)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("custo");

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

                entity.Property(e => e.Debreg)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("debreg");

                entity.Property(e => e.Debregm)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("debregm");

                entity.Property(e => e.Descar)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("descar")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Descc)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("descc");

                entity.Property(e => e.Descm)
                    .HasColumnType("numeric(13, 3)")
                    .HasColumnName("descm");

                entity.Property(e => e.Diaplano)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("diaplano");

                entity.Property(e => e.Diferido)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("diferido");

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
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Eancl)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("eancl")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Eanft)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("eanft")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Echtotal)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("echtotal");

                entity.Property(e => e.Ecusto)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ecusto");

                entity.Property(e => e.Edebreg)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("edebreg");

                entity.Property(e => e.Edescc)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("edescc");

                entity.Property(e => e.Ediferido)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ediferido");

                entity.Property(e => e.Efinv)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("efinv");

                entity.Property(e => e.Eivain1)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivain1");

                entity.Property(e => e.Eivain2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivain2");

                entity.Property(e => e.Eivain3)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivain3");

                entity.Property(e => e.Eivain4)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivain4");

                entity.Property(e => e.Eivain5)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivain5");

                entity.Property(e => e.Eivain6)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivain6");

                entity.Property(e => e.Eivain7)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivain7");

                entity.Property(e => e.Eivain8)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivain8");

                entity.Property(e => e.Eivain9)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eivain9");

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

                entity.Property(e => e.Encm)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("encm")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Encmdesc)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("encmdesc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Encomenda)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("encomenda")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Eportes)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("eportes");

                entity.Property(e => e.Erdtotal)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("erdtotal");

                entity.Property(e => e.Estab)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("estab");

                entity.Property(e => e.Etot1)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("etot1");

                entity.Property(e => e.Etot2)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("etot2");

                entity.Property(e => e.Etot3)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("etot3");

                entity.Property(e => e.Etot4)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("etot4");

                entity.Property(e => e.Etotal)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("etotal");

                entity.Property(e => e.Ettiliq)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ettiliq");

                entity.Property(e => e.Ettiva)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("ettiva");

                entity.Property(e => e.Evirs)
                    .HasColumnType("numeric(19, 6)")
                    .HasColumnName("evirs");

                entity.Property(e => e.Excm)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("excm");

                entity.Property(e => e.Excmdesc)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("excmdesc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Expedicao)
                    .HasMaxLength(28)
                    .IsUnicode(false)
                    .HasColumnName("expedicao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Facturada).HasColumnName("facturada");

                entity.Property(e => e.Fdata)
                    .HasColumnType("datetime")
                    .HasColumnName("fdata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Fin)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("fin");

                entity.Property(e => e.Final)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("final")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Finv)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("finv");

                entity.Property(e => e.Finvm)
                    .HasColumnType("numeric(13, 3)")
                    .HasColumnName("finvm");

                entity.Property(e => e.Fnoft)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("fnoft");

                entity.Property(e => e.Fref)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("fref")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ftid)
                    .HasColumnType("numeric(12, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ftid");

                entity.Property(e => e.Ftpos)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ftpos");

                entity.Property(e => e.Ftstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ftstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Idft).HasColumnName("idft");

                entity.Property(e => e.Iecacodisen)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("iecacodisen")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Iecadoccod)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("iecadoccod")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Iectisento).HasColumnName("iectisento");

                entity.Property(e => e.Impresso).HasColumnName("impresso");

                entity.Property(e => e.Intid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("intid")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Introfin).HasColumnName("introfin");

                entity.Property(e => e.Ivain1)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivain1");

                entity.Property(e => e.Ivain2)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivain2");

                entity.Property(e => e.Ivain3)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivain3");

                entity.Property(e => e.Ivain4)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivain4");

                entity.Property(e => e.Ivain5)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivain5");

                entity.Property(e => e.Ivain6)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivain6");

                entity.Property(e => e.Ivain7)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivain7");

                entity.Property(e => e.Ivain8)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivain8");

                entity.Property(e => e.Ivain9)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ivain9");

                entity.Property(e => e.Ivamin1)
                    .HasColumnType("numeric(16, 3)")
                    .HasColumnName("ivamin1");

                entity.Property(e => e.Ivamin2)
                    .HasColumnType("numeric(16, 3)")
                    .HasColumnName("ivamin2");

                entity.Property(e => e.Ivamin3)
                    .HasColumnType("numeric(16, 3)")
                    .HasColumnName("ivamin3");

                entity.Property(e => e.Ivamin4)
                    .HasColumnType("numeric(16, 3)")
                    .HasColumnName("ivamin4");

                entity.Property(e => e.Ivamin5)
                    .HasColumnType("numeric(16, 3)")
                    .HasColumnName("ivamin5");

                entity.Property(e => e.Ivamin6)
                    .HasColumnType("numeric(16, 3)")
                    .HasColumnName("ivamin6");

                entity.Property(e => e.Ivamin7)
                    .HasColumnType("numeric(16, 3)")
                    .HasColumnName("ivamin7");

                entity.Property(e => e.Ivamin8)
                    .HasColumnType("numeric(16, 3)")
                    .HasColumnName("ivamin8");

                entity.Property(e => e.Ivamin9)
                    .HasColumnType("numeric(16, 3)")
                    .HasColumnName("ivamin9");

                entity.Property(e => e.Ivamv1)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("ivamv1");

                entity.Property(e => e.Ivamv2)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("ivamv2");

                entity.Property(e => e.Ivamv3)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("ivamv3");

                entity.Property(e => e.Ivamv4)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("ivamv4");

                entity.Property(e => e.Ivamv5)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("ivamv5");

                entity.Property(e => e.Ivamv6)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("ivamv6");

                entity.Property(e => e.Ivamv7)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("ivamv7");

                entity.Property(e => e.Ivamv8)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("ivamv8");

                entity.Property(e => e.Ivamv9)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("ivamv9");

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

                entity.Property(e => e.Jaexpedi).HasColumnName("jaexpedi");

                entity.Property(e => e.Lang)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("lang")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Lifref).HasColumnName("lifref");

                entity.Property(e => e.Local)
                    .HasMaxLength(43)
                    .IsUnicode(false)
                    .HasColumnName("local")
                    .HasDefaultValueSql("('')");

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

                entity.Property(e => e.Marcada).HasColumnName("marcada");

                entity.Property(e => e.Matricula)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("matricula")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Meiost).HasColumnName("meiost");

                entity.Property(e => e.Memissao)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("memissao")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Mhstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("mhstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Moeda)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("moeda")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Morada)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("morada")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Mrstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("mrstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Multi).HasColumnName("multi");

                entity.Property(e => e.Ncin).HasColumnName("ncin");

                entity.Property(e => e.Ncont)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ncont")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ncout).HasColumnName("ncout");

                entity.Property(e => e.Ncusto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ncusto")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Niec)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("niec")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nmdoc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("nmdoc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Nmdocft)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("nmdocft")
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

                entity.Property(e => e.Nprotri).HasColumnName("nprotri");

                entity.Property(e => e.Ntcm)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ntcm");

                entity.Property(e => e.Optri).HasColumnName("optri");

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

                entity.Property(e => e.Pagamento)
                    .HasMaxLength(28)
                    .IsUnicode(false)
                    .HasColumnName("pagamento")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Pais)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("pais");

                entity.Property(e => e.Pdata)
                    .HasColumnType("datetime")
                    .HasColumnName("pdata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Peso)
                    .HasColumnType("numeric(14, 3)")
                    .HasColumnName("peso");

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

                entity.Property(e => e.Portes)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("portes");

                entity.Property(e => e.Procomss).HasColumnName("procomss");

                entity.Property(e => e.Pscm)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("pscm")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Pscmdesc)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("pscmdesc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ptcm)
                    .HasMaxLength(43)
                    .IsUnicode(false)
                    .HasColumnName("ptcm")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ptcmdesc)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("ptcmdesc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Qtt1)
                    .HasColumnType("numeric(16, 3)")
                    .HasColumnName("qtt1");

                entity.Property(e => e.Qtt2)
                    .HasColumnType("numeric(16, 3)")
                    .HasColumnName("qtt2");

                entity.Property(e => e.Qtt3)
                    .HasColumnType("numeric(16, 3)")
                    .HasColumnName("qtt3");

                entity.Property(e => e.Qtt4)
                    .HasColumnType("numeric(16, 3)")
                    .HasColumnName("qtt4");

                entity.Property(e => e.Rdtotal)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("rdtotal");

                entity.Property(e => e.Rdtotalm)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("rdtotalm");

                entity.Property(e => e.Rota)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("rota")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Rpcldfim)
                    .HasColumnType("datetime")
                    .HasColumnName("rpcldfim")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Rpcldini)
                    .HasColumnType("datetime")
                    .HasColumnName("rpcldini")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.Rpclnome)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("rpclnome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Rpclstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("rpclstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Saida)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("saida")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Segmento)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("segmento")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Series)
                    .HasColumnType("text")
                    .HasColumnName("series")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Series2)
                    .HasColumnType("text")
                    .HasColumnName("series2")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Site)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("site")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Snstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("snstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

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

                entity.Property(e => e.Telefone)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("telefone")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tipo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tipodoc)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("tipodoc");

                entity.Property(e => e.Tmiliq)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("tmiliq");

                entity.Property(e => e.Tmiva)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("tmiva");

                entity.Property(e => e.Tot1)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("tot1");

                entity.Property(e => e.Tot2)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("tot2");

                entity.Property(e => e.Tot3)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("tot3");

                entity.Property(e => e.Tot4)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("tot4");

                entity.Property(e => e.Total)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("total");

                entity.Property(e => e.Totalmoeda)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("totalmoeda");

                entity.Property(e => e.Totqtt)
                    .HasColumnType("numeric(15, 3)")
                    .HasColumnName("totqtt");

                entity.Property(e => e.Tpdesc)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("tpdesc")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tpstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("tpstamp")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.Tptit)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("tptit")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ttiliq)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ttiliq");

                entity.Property(e => e.Ttiva)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("ttiva");

                entity.Property(e => e.UAltft)
                    .HasColumnType("text")
                    .HasColumnName("u_altft")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UCambio)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_cambio");

                entity.Property(e => e.UCodaut)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_codaut")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UDnp)
                    .HasColumnType("datetime")
                    .HasColumnName("u_dnp")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101',0))");

                entity.Property(e => e.UIdpac)
                    .HasColumnType("numeric(9, 0)")
                    .HasColumnName("u_idpac");

                entity.Property(e => e.UNbf)
                    .HasMaxLength(18)
                    .IsUnicode(false)
                    .HasColumnName("u_nbf")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UNomepac)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("u_nomepac")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Usaintra).HasColumnName("usaintra");

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

                entity.Property(e => e.Valorm2)
                    .HasColumnType("numeric(15, 2)")
                    .HasColumnName("valorm2");

                entity.Property(e => e.Vendedor)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("vendedor");

                entity.Property(e => e.Vendnm)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("vendnm")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Virs)
                    .HasColumnType("numeric(18, 5)")
                    .HasColumnName("virs");

                entity.Property(e => e.Zncm)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("zncm");

                entity.Property(e => e.Znregiao)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("znregiao")
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

                entity.Property(e => e.UAnomal)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("u_anomal")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UCalibre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_calibre")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UCalpstp)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("u_calpstp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UContador)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_contador")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UEtotliq)
                    .HasColumnType("numeric(16, 5)")
                    .HasColumnName("u_etotliq");

                entity.Property(e => e.UFactu)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("u_factu");

                entity.Property(e => e.UFactura)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("u_factura");

                entity.Property(e => e.UGestcont)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_gestcont")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UGestnome)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_gestnome")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ULeiact)
                    .HasColumnType("numeric(16, 0)")
                    .HasColumnName("u_leiact");

                entity.Property(e => e.ULeiant)
                    .HasColumnType("numeric(16, 0)")
                    .HasColumnName("u_leiant");

                entity.Property(e => e.UMultado).HasColumnName("u_multado");

                entity.Property(e => e.UOfdata)
                    .HasColumnType("datetime")
                    .HasColumnName("u_ofdata")
                    .HasDefaultValueSql("(CONVERT([datetime],'19000101'))");

                entity.Property(e => e.UOftstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_oftstamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UOristamp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_oristamp")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UPeriodo)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("u_periodo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UReal)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("u_real");

                entity.Property(e => e.UTipofac)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("u_tipofac")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UTotliq)
                    .HasColumnType("numeric(16, 5)")
                    .HasColumnName("u_totliq");

                entity.Property(e => e.UTotliqm)
                    .HasColumnType("numeric(16, 5)")
                    .HasColumnName("u_totliqm");

                entity.Property(e => e.UTx1)
                    .HasColumnType("numeric(16, 2)")
                    .HasColumnName("u_tx1");

                entity.Property(e => e.UWwfid)
                    .HasColumnType("numeric(12, 0)")
                    .HasColumnName("u_wwfid");

                entity.Property(e => e.UWwfstamp)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("u_wwfstamp")
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


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}


//AppDbContext