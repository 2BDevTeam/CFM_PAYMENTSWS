using CFM_PAYMENTSWS.Domains.Models;
using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.DTOs
{
    public class PagamentoResultDTO
    {
        public List<ReciboAux> recibos;

        public List<Cc> contaCorrente;

        public PagamentoResultDTO(List<ReciboAux> recibos, List<Cc> contaCorrente)
        {
            this.recibos = recibos;
            this.contaCorrente = contaCorrente;
        }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
