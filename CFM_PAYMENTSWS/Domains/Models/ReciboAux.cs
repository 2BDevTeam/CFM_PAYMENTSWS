using CFM_PAYMENTSWS.Domains.Models.Enum;
using CFM_PAYMENTSWS.DTOs;

namespace CFM_PAYMENTSWS.Domains.Models
{
    public class ReciboAux
    {
        public ReciboAux(string stamp, decimal numeroCliente, string nomeCliente, TipoReciboEnum tipo, decimal total, decimal totalFacturaCorrente, decimal totalDividas, ResponseCodesDTO paymentResponse)
        {
            this.stamp = stamp;
            this.numeroCliente = numeroCliente;
            this.nomeCliente = nomeCliente;
            this.tipo = tipo;
            this.total = total;
            this.totalFacturaCorrente = totalFacturaCorrente;
            this.totalDividas = totalDividas;
            this.paymentResponse = paymentResponse;
        }

        public string stamp { get; set; }
        /// <summary>
        /// Numero do cliente
        /// </summary>
        public decimal numeroCliente { get; set; }
        /// <summary>
        /// Nome do cliente
        /// </summary>
        public string nomeCliente { get; set; }
        /// <summary>
        /// Tipo do recibo
        /// </summary>
        public TipoReciboEnum tipo { get; set; }
        /// <summary>
        /// Total do recibo
        /// </summary>
        public decimal total { get; set; }

        public decimal totalFacturaCorrente { get; set; }


        public decimal totalDividas { get; set; }

        public ResponseCodesDTO paymentResponse { get; set; }
    }
}
