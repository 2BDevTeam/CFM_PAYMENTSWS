using System;
using System.Collections.Generic;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public partial class Bl
    {
        public string Blstamp { get; set; } = null!;
        public string Banco { get; set; } = null!;
        public string Conta { get; set; } = null!;
        public string Nib { get; set; } = null!;
        public decimal Noconta { get; set; }
        public DateTime Datasaldo { get; set; }
        public decimal Saldo { get; set; }
        public string Moeda { get; set; } = null!;
        public string Dados { get; set; } = null!;
        public string Cpoc { get; set; } = null!;
        public bool Caucionada { get; set; }
        public string Estab { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public decimal Esaldo { get; set; }
        public string Telefone { get; set; } = null!;
        public string Fax { get; set; } = null!;
        public string Email { get; set; } = null!;
        public decimal Rsaldo { get; set; }
        public decimal Ersaldo { get; set; }
        public string Gestor { get; set; } = null!;
        public bool Mensaldo { get; set; }
        public decimal Lrplaf { get; set; }
        public decimal Elrplaf { get; set; }
        public string Ousrinis { get; set; } = null!;
        public DateTime Ousrdata { get; set; }
        public string Ousrhora { get; set; } = null!;
        public string Usrinis { get; set; } = null!;
        public DateTime Usrdata { get; set; }
        public string Usrhora { get; set; } = null!;
        public bool Marcada { get; set; }
        public bool UEurmo { get; set; }
        public string UMoe { get; set; } = null!;
        public decimal Saldom { get; set; }
        public decimal Rsaldom { get; set; }
        public decimal Lrplafm { get; set; }
        public decimal Cjuro { get; set; }
        public decimal Cplafond { get; set; }
        public decimal Ecplafond { get; set; }
        public decimal Cplafondm { get; set; }
        public decimal Ordem { get; set; }
        public bool Inactivo { get; set; }
        public string UCcusto { get; set; } = null!;
        public bool UCntfixo { get; set; }
        public decimal UDias { get; set; }
        public decimal UMntfixo { get; set; }
        public decimal UMntmaxd { get; set; }
        public decimal UDino { get; set; }
        public string UDinome { get; set; } = null!;
        public bool UVerborc { get; set; }
        public string Titular { get; set; } = null!;
        public string Codigo { get; set; } = null!;
        public string Codigo2 { get; set; } = null!;
        public bool Online { get; set; }
        public string Contaletdes { get; set; } = null!;
        public string Codigopre { get; set; } = null!;
        public string Codigopre2 { get; set; } = null!;
        public string Entidade { get; set; } = null!;
        public string Sucursal { get; set; } = null!;
        public string Dc { get; set; } = null!;
        public string Nconta { get; set; } = null!;
        public bool Contautil { get; set; }
        public string Bic { get; set; } = null!;
        public string Iban { get; set; } = null!;
        public string E1initgpty { get; set; } = null!;
        public string Tipoconta { get; set; } = null!;
        public string Codpais { get; set; } = null!;
        public string Pais { get; set; } = null!;
        public bool Entnac { get; set; }
        public decimal Idbanco { get; set; }
        public string UBancagr { get; set; } = null!;
        public string UContaba { get; set; } = null!;
        public DateTime UDtctc { get; set; }
        public DateTime UDtvmt { get; set; }
        public decimal UTaxa { get; set; }
        public decimal ULcs { get; set; }
        public decimal UCaucao { get; set; }
        public decimal UFinanca { get; set; }
        public decimal UReserva { get; set; }
        public decimal UCodeprov { get; set; }
        public bool Valdata { get; set; }
        public bool Valdatav { get; set; }
        public bool Valdesc { get; set; }
        public bool Valmovant { get; set; }
        public decimal Tvaldata { get; set; }
        public decimal Tvaldatav { get; set; }
        public bool Fundomaneio { get; set; }
        public decimal Saldofundomaneio { get; set; }
        public decimal Periododias { get; set; }
        public string Contaprincipal { get; set; } = null!;
        public DateTime Datacarregamento { get; set; }
        public string Accountid { get; set; } = null!;
        public string Accountname { get; set; } = null!;
        public bool Coverflex { get; set; }
        public string Precisao { get; set; } = null!;
        public decimal Precisaoid { get; set; }
    }
}
