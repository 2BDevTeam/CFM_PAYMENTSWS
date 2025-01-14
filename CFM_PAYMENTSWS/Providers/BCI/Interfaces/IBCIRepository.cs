using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.Providers.BCI.DTOs;

namespace CFM_PAYMENTSWS.Providers.BCI.Interfaces
{
    public interface IBCIRepository
    {
        BCIResponseDTO loadPayments(U2bPaymentsQueue u2BPaymentsQueue);
    }
}
