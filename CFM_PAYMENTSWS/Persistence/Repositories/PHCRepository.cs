using Microsoft.EntityFrameworkCore;
using CFM_PAYMENTSWS.Domains.Interfaces;
using CFM_PAYMENTSWS.Domains.Models;
using System.Linq.Dynamic.Core;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Extensions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using CFM_PAYMENTSWS.Helper;
using System.Globalization;

namespace CFM_PAYMENTSWS.Persistence.Repositories
{
    public class PHCRepository<TContext> : IPHCRepository<TContext> where TContext : DbContext
    {

        private readonly ConversionExtension conversionExtension = new ConversionExtension();
        private readonly TContext _context;

        public PHCRepository(TContext context)
        {
            _context = context;
        }


        /*
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

        */

    }
}

//PHCRepository