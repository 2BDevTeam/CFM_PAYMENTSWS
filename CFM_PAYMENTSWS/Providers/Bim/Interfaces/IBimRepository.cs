using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.Providers.Bim.DTOs;

namespace CFM_PAYMENTSWS.Providers.Bim.Interfaces
{
    public interface IBimRepository
    {
        BimResponseDTO loadPayments(U2bPaymentsQueue u2BPaymentsQueue);
    }
}
