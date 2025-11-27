using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CFM_PAYMENTSWS.Domains.Interfaces
{
    public interface IPHCRepository<TContext> where TContext : DbContext
    {

        public List<UProvider> GetUProvider(decimal code);
        public JobLocks GetJobLocks(string jobId);
        public Po GetPo(string postamp);
        public Pd GetPd(string pdstamp);
        public Ol GetOl(string olstamp);
        public Ow GetOw(string owstamp);
        public Tb GetTb(string tbstamp);

        public UTrfb GetUTrfb(string trfbstamp);
        public UWspayments GetWspaymentsByStamp(string stamp);
        public List<Liame> GetLiameProcessado(bool processado);
        public List<UWspayments> GetWspayments(string batchid);
        public UWspayments GetWspaymentsByDestino(string batchid, string destino);

        Task<Ft?> GetFtByRef(string no);
        Task<Cl> getClienteByNo(decimal no);
        Task<List<Cc>> getContaCorrenteByStamp(string stamp);

        decimal getMaxRecibo();
        void addRecibo(Re recibocc);
        void addLinhasRecibo(Rl linhasRecibo);
        void addTitulos(Rech titulo);
        Tsre getConfiguracaoRecibo();
        
        Bl getBlByBancagr(string bancagroup);

        string getMoeda();

    }
}
