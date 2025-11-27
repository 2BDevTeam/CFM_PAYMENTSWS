using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models;

public partial class Cl
{
    public string Clstamp { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public decimal No { get; set; }

    public decimal Estab { get; set; }

    public string Vendnm { get; set; } = null!;

    public string Ncont { get; set; } = null!;

    public string Nome2 { get; set; } = null!;

    public decimal Saldo { get; set; }

    public decimal Esaldo { get; set; }

    public string Moeda { get; set; } = null!;

    public string Fax { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public string Contacto { get; set; } = null!;

    public decimal Acmfact { get; set; }

    public decimal Eacmfact { get; set; }

    public decimal Rentval { get; set; }

    public decimal Erentval { get; set; }

    public bool Eem { get; set; }

    public decimal Emno { get; set; }

    public bool Eag { get; set; }

    public decimal Agno { get; set; }

    public bool Eid { get; set; }

    public decimal Idno { get; set; }

    public bool Efl { get; set; }

    public decimal Flno { get; set; }

    public decimal Flestab { get; set; }

    public string Morada { get; set; } = null!;

    public string Local { get; set; } = null!;

    public string Codpost { get; set; } = null!;

    public string Zona { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public decimal Desconto { get; set; }

    public decimal Vendedor { get; set; }

    public decimal Vencimento { get; set; }

    public decimal Plafond { get; set; }

    public decimal Eplafond { get; set; }

    public string Obs { get; set; } = null!;

    public decimal Preco { get; set; }

    public decimal Pais { get; set; }

    public bool Particular { get; set; }

    public string Bino { get; set; } = null!;

    public DateTime Bidata { get; set; }

    public string Bilocal { get; set; } = null!;

    public string Naturalid { get; set; } = null!;

    public string Passaporte { get; set; } = null!;

    public string Conta { get; set; } = null!;

    public DateTime Nascimento { get; set; }

    public string Pagamento { get; set; } = null!;

    public string Cobranca { get; set; } = null!;

    public string Nib { get; set; } = null!;

    public decimal Descpp { get; set; }

    public string Imagem { get; set; } = null!;

    public decimal Odatraso { get; set; }

    public decimal Tabiva { get; set; }

    public string C1tele { get; set; } = null!;

    public string C1fax { get; set; } = null!;

    public string C1email { get; set; } = null!;

    public string C1func { get; set; } = null!;

    public string C2tele { get; set; } = null!;

    public string C2fax { get; set; } = null!;

    public string C2email { get; set; } = null!;

    public string C2func { get; set; } = null!;

    public string C2tacto { get; set; } = null!;

    public string C3tele { get; set; } = null!;

    public string C3fax { get; set; } = null!;

    public string C3email { get; set; } = null!;

    public string C3func { get; set; } = null!;

    public string C3tacto { get; set; } = null!;

    public bool Dqtt { get; set; }

    public bool Clivd { get; set; }

    public string Descarga { get; set; } = null!;

    public bool Nocredit { get; set; }

    public string Segmento { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Fref { get; set; } = null!;

    public string Ccusto { get; set; } = null!;

    public string Ncusto { get; set; } = null!;

    public bool Naood { get; set; }

    public bool Naomail { get; set; }

    public string Contalet { get; set; } = null!;

    public string Contaletdes { get; set; } = null!;

    public string Contaletsac { get; set; } = null!;

    public decimal Alimite { get; set; }

    public decimal Dqttval { get; set; }

    public string Tipodesc { get; set; } = null!;

    public string Tlmvl { get; set; } = null!;

    public string Cobrador { get; set; } = null!;

    public string Rota { get; set; } = null!;

    public string Contaainc { get; set; } = null!;

    public string Contaacer { get; set; } = null!;

    public string Contamovinc { get; set; } = null!;

    public string Contadivinc { get; set; } = null!;

    public string Eancl { get; set; } = null!;

    public bool Ediexp { get; set; }

    public string Url { get; set; } = null!;

    public string Tpstamp { get; set; } = null!;

    public string Tpdesc { get; set; } = null!;

    public string Pncont { get; set; } = null!;

    public string Cobtele { get; set; } = null!;

    public string Cobfax { get; set; } = null!;

    public string Cobemail { get; set; } = null!;

    public string Cobfunc { get; set; } = null!;

    public string Cobtacto { get; set; } = null!;

    public string Ollocal { get; set; } = null!;

    public decimal Contado { get; set; }

    public DateTime Pcktsyncdate { get; set; }

    public string Pcktsynctime { get; set; } = null!;

    public string Pscm { get; set; } = null!;

    public decimal Zncm { get; set; }

    public decimal Excm { get; set; }

    public string Ptcm { get; set; } = null!;

    public string Encm { get; set; } = null!;

    public decimal Ntcm { get; set; }

    public string Pscmdesc { get; set; } = null!;

    public string Znregiao { get; set; } = null!;

    public string Excmdesc { get; set; } = null!;

    public string Ptcmdesc { get; set; } = null!;

    public string Encmdesc { get; set; } = null!;

    public bool Cobnao { get; set; }

    public decimal Saldlet { get; set; }

    public decimal Esaldlet { get; set; }

    public string Site { get; set; } = null!;

    public string Bizzaddress { get; set; } = null!;

    public decimal Bizzproto { get; set; }

    public string Cass { get; set; } = null!;

    public string Classe { get; set; } = null!;

    public string Lang { get; set; } = null!;

    public bool Iectisento { get; set; }

    public string Niec { get; set; } = null!;

    public string Gaenome { get; set; } = null!;

    public string Gaecstamp { get; set; } = null!;

    public bool Clinica { get; set; }

    public bool Ftndias { get; set; }

    public string Txftndias { get; set; } = null!;

    public bool Ftdiasmr { get; set; }

    public string Txftdias { get; set; } = null!;

    public bool Ftdatasmr { get; set; }

    public string Txftdata { get; set; } = null!;

    public bool Ftnid { get; set; }

    public string Txftnid { get; set; } = null!;

    public bool Ftidnome { get; set; }

    public string Txftidnome { get; set; } = null!;

    public bool Ftidcontacto { get; set; }

    public string Txftidcontacto { get; set; } = null!;

    public bool Ftidnac { get; set; }

    public string Txftidnac { get; set; } = null!;

    public bool Ftidcont { get; set; }

    public string Txftidcont { get; set; } = null!;

    public bool Ftidutente { get; set; }

    public string Txftidutente { get; set; } = null!;

    public bool Ftidbi { get; set; }

    public string Txftidbi { get; set; } = null!;

    public bool Ftidcob { get; set; }

    public string Txftidcob { get; set; } = null!;

    public bool Ftmrtot { get; set; }

    public string Txftmrtot { get; set; } = null!;

    public bool Ftumamr { get; set; }

    public bool Paramr { get; set; }

    public bool Filtrast { get; set; }

    public string Contatit { get; set; } = null!;

    public string Statuspda { get; set; } = null!;

    public bool Inactivo { get; set; }

    public bool Naoencomenda { get; set; }

    public bool Clifactor { get; set; }

    public string Contafac { get; set; } = null!;

    public bool Dfront { get; set; }

    public bool Dsuporte { get; set; }

    public bool Dformacao { get; set; }

    public bool Dteam { get; set; }

    public bool Recdocdig { get; set; }

    public string Glncl { get; set; } = null!;

    public string Codfornecedor { get; set; } = null!;

    public string Localentrega { get; set; } = null!;

    public string Obsdoc { get; set; } = null!;

    public bool Ecoisento { get; set; }

    public string Tbprcod { get; set; } = null!;

    public string Area { get; set; } = null!;

    public bool Exporpos { get; set; }

    public bool Cancpos { get; set; }

    public bool Temcred { get; set; }

    public bool Temftglob { get; set; }

    public decimal Ltyp { get; set; }

    public decimal Lmlt { get; set; }

    public decimal Rbal { get; set; }

    public bool Addd { get; set; }

    public string Id { get; set; } = null!;

    public string Track { get; set; } = null!;

    public decimal Tracknr { get; set; }

    public string Pin { get; set; } = null!;

    public string Encrpin { get; set; } = null!;

    public bool Blck { get; set; }

    public decimal Acc { get; set; }

    public bool Repl { get; set; }

    public bool Odo { get; set; }

    public bool Did { get; set; }

    public bool Carr { get; set; }

    public string Fuels { get; set; } = null!;

    public bool Cw { get; set; }

    public string Shop { get; set; } = null!;

    public string Refcli { get; set; } = null!;

    public string Matric { get; set; } = null!;

    public decimal Desccmb { get; set; }

    public decimal Descloj { get; set; }

    public bool Isperson { get; set; }

    public decimal Radicaltipoemp { get; set; }

    public string Numcontrepres { get; set; } = null!;

    public string Codprovincia { get; set; } = null!;

    public bool Autorizacaoactiva { get; set; }

    public string Numautorizacaosdd { get; set; } = null!;

    public decimal Numseqaut { get; set; }

    public bool Autofact { get; set; }

    public string Mesesnaopag { get; set; } = null!;

    public string Diaspag { get; set; } = null!;

    public string Descregiva { get; set; } = null!;

    public string Motiseimp { get; set; } = null!;

    public string Codmotiseimp { get; set; } = null!;

    public string Distrito { get; set; } = null!;

    public bool Ccadmin { get; set; }

    public bool Geramb { get; set; }

    public string Bic { get; set; } = null!;

    public string Iban { get; set; } = null!;

    public DateTime Datasdd { get; set; }

    public string Sepacode { get; set; } = null!;

    public bool Operext { get; set; }

    public bool Consfinal { get; set; }

    public decimal Saldopa { get; set; }

    public decimal Saldoini { get; set; }

    public decimal Taxairs { get; set; }

    public bool Txirspersonalizada { get; set; }

    public string Ousrinis { get; set; } = null!;

    public DateTime Ousrdata { get; set; }

    public string Ousrhora { get; set; } = null!;

    public string Usrinis { get; set; } = null!;

    public DateTime Usrdata { get; set; }

    public string Usrhora { get; set; } = null!;

    public bool Marcada { get; set; }
}
