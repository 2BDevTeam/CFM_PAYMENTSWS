using Microsoft.EntityFrameworkCore;
using LENMEDWS.Domains.Interfaces;
using LENMEDWS.Domains.Models;
using System.Linq.Dynamic.Core;
using LENMEDWS.DTOs;
using LENMEDWS.Extensions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using LENMEDWS.Helper;

namespace LENMEDWS.Persistence.Repositories
{
    public class PHCRepository<TContext> : IPHCRepository<TContext> where TContext : DbContext
    {

        private readonly ConversionExtension conversionExtension = new ConversionExtension();
        private readonly TContext _context;

        public PHCRepository(TContext context)
        {
            _context = context;
        }



        public Ft GetFt(string ftstamp)
        {
            return _context.Set<Ft>().
                FirstOrDefault(ft => ft.Ftstamp == ftstamp);
        }

        public Ft2 GetFt2(string ft2stamp)
        {
            return _context.Set<Ft2>().
                FirstOrDefault(ft => ft.Ft2stamp == ft2stamp);
        }


        public Para1 GetPara1(string descricao)
        {
            return _context.Set<Para1>().
                FirstOrDefault(p1 => p1.Descricao == descricao);
        }

        public List<Ml> GetMlByDate(DateTime initialDate, DateTime finalDate)
        {
            return _context.Set<Ml>()
                .ToList();
        }


        public Para1 GetPara1ByDescricao(string descricao)
        {
            return _context.Set<Para1>()
                .FirstOrDefault(p1 => p1.Descricao == descricao);
        }


        public E1 GetE1ByNomecomp(string nome)
        {
            return _context.Set<E1>()
                .FirstOrDefault(p1 => p1.Nomecomp == nome);
        }

        public List<GLTransactionsDTO> GetGLTransactions(DateTime initialDate, DateTime finalDate, string ncont, DateTime mcDataf, int? pageIndex, int? pageSize)
        {
            /*
            var query = _context.Set<Ml>()
            .Join(
                _context.Set<Do>(),
                ml => ml.Dostamp,
                doEntity => doEntity.Dostamp,
                (ml, doEntity) => new { Ml = ml, Do = doEntity }
            )
            .Join(
                _context.Set<Cu>(),
                joined => joined.Ml.Cct,
                cu => cu.Cct,
                (joined, cu) => new { joined.Ml, joined.Do, Cu = cu }
            )
            .Where(joined =>
                    joined.Ml.Data >= initialDate
                    && joined.Ml.Data <= finalDate
                    )
            .Select(joined => new GLTransactionsDTO
            {
                HospitalCode = "",
                CaseNumber = 0,
                PostingDate = joined.Ml.Data.ToString("yyyy-MM-dd HH:mm:ss"),
                GLAccount = (int)joined.Ml.Dilno,
                GLAccountDesc = joined.Ml.Dinome,
                CostCenter = joined.Cu.Cct,
                CostCenterDesc = joined.Cu.Descricao,
                DCIndicator = joined.Ml.Deb == 0 ? "H" : (joined.Ml.Cre == 0 ? "S" : ""),
                Amount = Math.Round(joined.Ml.Deb != 0 ? joined.Ml.Deb : (joined.Ml.Cre != 0 ? joined.Ml.Cre : 0), 2),
                Currency = "MT"
            });
            */

            //CaseNumber = (int)(mr != null ? mr.Mrno : (ft != null ? ft.Fno : 0)),

            var query = from ml in _context.Set<Ml>()
                        join doEntity in _context.Set<Do>() on ml.Dostamp equals doEntity.Dostamp
                        join cu in _context.Set<Cu>() on ml.Cct equals cu.Cct
                        join ft in _context.Set<Ft>() on ml.Oristamp equals ft.Ftstamp into ftGroup
                        from ft in ftGroup.DefaultIfEmpty()
                        join mr in _context.Set<Mr>() on ft.Mrstamp equals mr.Mrstamp into mrGroup
                        from mr in mrGroup.DefaultIfEmpty()
                        where ml.Data >= initialDate && ml.Data <= finalDate 
                            && ml.Data <= mcDataf
                            && (mr != null || (ft != null && ft.Fno != 0))
                        orderby ml.Data ascending, ml.Dostamp descending, ml.Lordem ascending
                        select new GLTransactionsDTO
                        {
                            HospitalCode = ncont,
                            CaseNumber = (int)(mr != null ? mr.Mrno : (ft != null ? ft.Fno : 0)),
                            PostingDate = ml.Data.ToString("yyyy-MM-dd HH:mm:ss"),
                            LedgerId= (int)ml.Dilno,
                            GLAccount = ml.Conta,
                            GLAccountDesc = ml.Descricao.Trim(),
                            CostCenter = cu.Cct,
                            CostCenterDesc = cu.Descricao,
                            DCIndicator = ml.Deb == 0 ? "H" : (ml.Cre == 0 ? "S" : ""),
                            Amount = decimal.Parse(Math.Round(ml.Deb != 0 ? ml.Deb : (ml.Cre != 0 ? ml.Cre : 0), 2).ToString("F2")),
                            Currency = "MZN"
                        };



            List<GLTransactionsDTO> result = GenericRepository<TContext>.PaginateQueryResult(query, pageIndex, pageSize);

            return result;
        }


        public int GetGLTransactionsCount(DateTime initialDate, DateTime finalDate)
        {
            return _context.Set<Ml>()
            .Join(
                _context.Set<Do>(),
                ml => ml.Dostamp,
                doEntity => doEntity.Dostamp,
                (ml, doEntity) => new { Ml = ml, Do = doEntity }
            )
            .Join(
                _context.Set<Cu>(),
                joined => joined.Ml.Cct,
                cu => cu.Cct,
                (joined, cu) => new { joined.Ml, joined.Do, Cu = cu }
            )
            .Count(joined =>
                    joined.Ml.Data >= initialDate
                    && joined.Ml.Data <= finalDate
                    );

        }


    }
}

//KOBORepository