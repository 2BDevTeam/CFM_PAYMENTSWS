using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CFM_PAYMENTSWS.Domains.Interface
{
    public interface IPaymentRepository<TContext> where TContext : DbContext
    {
        public List<PaymentsQueue> GetPagamentQueue(string estado);
        public U2bPaymentsTs GetPayment(string transactionId, string batchId);
        public List<U2bPaymentsTs> GetPaymentsBatchId(string batchId);
        public List<U2bPaymentsQueueTs> GetPaymentsQueueBatchId(string batchId);
        public bool verificaBatchId(string batchId);
    }
}
