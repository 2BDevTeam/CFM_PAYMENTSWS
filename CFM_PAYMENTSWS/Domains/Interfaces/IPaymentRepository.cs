using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CFM_PAYMENTSWS.Domains.Interface
{
    public interface IPaymentRepository<TContext> where TContext : DbContext
    {
        public List<PaymentsQueue> GetPagamentQueue(string estado);
        public U2bPayments GetPayment(string transactionId, string batchId);
        public List<U2bPayments> GetPaymentsBatchId(string batchId);
        public List<U2bPaymentsQueue> GetPaymentsQueueBatchId(string batchId);
        public bool verificaBatchId(string batchId);
        public List<UProvider> getProviderData(decimal providerCode);
        public List<UProvider> getProviderByGroup(decimal providerCode, string grupo);
        public Suliame getUserEmail(int userno);


    }
}
