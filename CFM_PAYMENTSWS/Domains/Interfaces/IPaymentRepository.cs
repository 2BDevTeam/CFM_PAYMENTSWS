using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CFM_PAYMENTSWS.Domains.Interface
{
    public interface IPaymentRepository<TContext> where TContext : DbContext
    {
        public void actualizarEstadoDoPagamento(U2bPaymentsQueue u2BPayments, ResponseDTO responseDTO);
        public List<U2bPaymentsQueue> GetPagamentosEmFila(string estado, decimal canal);
        public Task<List<PaymentsQueue>> GetPagamentQueue(string estado, decimal canal);
        public List<int?> GetCanais_UPaymentQueue();
        public U2bPayments GetPayment(string transactionId, string batchId);
        public U2bPayments GetPaymentByStamp(string u2bPaymentsStamp);
        public List<U2bPayments> GetPaymentsBatchId(string batchId);
        public List<U2bPaymentsQueue> GetPaymentsQueueBatchId(string batchId);
        public bool verificaBatchId(string batchId);
        public List<UProvider> getProviderData(decimal providerCode);
        public List<UProvider> getProviderByGroup(decimal providerCode, string grupo);

        Task<bool> PaymentExistsAsync(U2bRecPayments payment);
        List<U2bRecPayments> GetPendingTransactions();
        Task AddPayment(U2bRecPayments payment);
        void updateTransactionStatus(U2bRecPayments transacaoActualizar);
        U2bRecPayments GetU2BRecPayments(string id);

    }
}
