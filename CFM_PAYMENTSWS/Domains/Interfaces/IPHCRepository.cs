using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CFM_PAYMENTSWS.Domains.Interfaces
{
    public interface IPHCRepository<TContext> where TContext : DbContext
    {

        public JobLocks GetJobLocks(string jobId);
        public Po GetPo(string postamp);
        public Pd GetPd(string pdstamp);
        public Ol GetOl(string olstamp);
        public Ow GetOW(string owstamp);
        public List<Liame> GetLiameProcessado(bool processado);
        public List<UWspayments> GetWspayments(string batchid);
        public UWspayments GetWspaymentsByDestino(string batchid, string destino);
        public string SendEmail(string email, string subject, string body);
        public string GetFullBody(string corpo);
        /*
        public Ft GetFt(string ftstamp);
        public Ft2 GetFt2(string ft2stamp);

        public Para1 GetPara1(string descricao);
        public List<Ml> GetMlByDate(DateTime initialDate, DateTime finalDate);
        public Para1 GetPara1ByDescricao(string descricao);
        public E1 GetE1ByEstab(int estab);
        */
    }
}
