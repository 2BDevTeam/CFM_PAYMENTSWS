using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;

namespace CFM_PAYMENTSWS.Providers.Nedbank.Interfaces
{
    public interface INedbankRepository
    {
        NedbankResponseDTO loadPayments(U2BPaymentsQueue u2BPaymentsQueue);
    }
}
