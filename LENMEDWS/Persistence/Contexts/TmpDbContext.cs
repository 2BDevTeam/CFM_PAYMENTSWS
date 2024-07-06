using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using LENMEDWS.Domains.Models;

namespace LENMEDWS.Persistence.Contexts
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

        public virtual DbSet<Mr> Mr { get; set; } = null!;

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
