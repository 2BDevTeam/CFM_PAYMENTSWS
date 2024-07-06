using LENMEDWS.Domains.Models;
using LENMEDWS.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LENMEDWS.Domains.Interfaces
{
    public interface IPHCRepository<TContext> where TContext : DbContext
    {

        public Ft GetFt(string ftstamp);
        public Ft2 GetFt2(string ft2stamp);

        public Para1 GetPara1(string descricao);
        public List<Ml> GetMlByDate(DateTime initialDate, DateTime finalDate);
        public Para1 GetPara1ByDescricao(string descricao);
        public E1 GetE1ByNomecomp(string nome);

        public List<GLTransactionsDTO> GetGLTransactions(DateTime initialDate, DateTime finalDate, string ncont, DateTime mcDataf, int? pageIndex, int? pageSize);

        public int GetGLTransactionsCount(DateTime initialDate, DateTime finalDate);

    }
}
