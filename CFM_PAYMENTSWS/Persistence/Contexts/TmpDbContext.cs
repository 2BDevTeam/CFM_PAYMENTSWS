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

        public virtual DbSet<Ow> Ow { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=NACALADESENV;Database=OnBD_CFM_32;User Id=2badmin.imacinga;MultipleActiveResultSets=true;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
