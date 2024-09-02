using CFM_PAYMENTSWS.DTOs;

namespace CFM_PAYMENTSWS.Domains.Interfaces
{
    public interface IPaymentService
    {
        public Task<ResponseDTO> actualizarPagamentos(PaymentCheckedDTO paymentHeader);
        /*
        public ResponseDTO GetResult(string nome);
        public ResponseDTO RegistarCliente(int id);
        */

        //public Task<ResponseDTO> GetGLTransactions(string inicialDate, string finDate, int? pageIndex, int? pageSize);
    }
}
