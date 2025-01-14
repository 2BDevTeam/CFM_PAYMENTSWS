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
using Microsoft.Data.SqlClient;
using CFM_PAYMENTSWS.Persistence.Contexts;

namespace CFM_PAYMENTSWS.Persistence.Repositories
{
    public class PHCRepository<TContext> : IPHCRepository<TContext> where TContext : DbContext
    {

        private readonly ConversionExtension conversionExtension = new ConversionExtension();
        EncryptionHelper encryptionHelper = new EncryptionHelper();
        private readonly TContext _context;

        public PHCRepository(TContext context)
        {
            _context = context;
        }

        public List<UProvider> GetUProvider(decimal code)
        {
            return _context.Set<UProvider>().
                Where(ft => ft.codigo == code)
                .ToList();
        }

        public JobLocks GetJobLocks(string jobId)
        {
            return _context.Set<JobLocks>().
                FirstOrDefault(ft => ft.JobId == jobId);
        }


        public Po GetPo(string postamp)
        {
            return _context.Set<Po>().
                FirstOrDefault(po => po.Postamp == postamp);
        }

        public Pd GetPd(string pdstamp)
        {
            return _context.Set<Pd>().
                FirstOrDefault(po => po.Pdstamp == pdstamp);
        }

        public Ol GetOl(string olstamp)
        {
            return _context.Set<Ol>().
                FirstOrDefault(po => po.Olstamp == olstamp);
        }

        public Ow GetOw(string owstamp)
        {
            return _context.Set<Ow>().
                FirstOrDefault(po => po.Owstamp == owstamp);
        }

        public Tb GetTb(string tbstamp)
        {
            return _context.Set<Tb>().
                FirstOrDefault(po => po.Tbstamp == tbstamp);
        }

        public UTrfb GetUTrfb(string trfbstamp)
        {
            return _context.Set<UTrfb>().
                FirstOrDefault(po => po.UTrfbstamp == trfbstamp);
        }

        public List<Liame> GetLiameProcessado(bool processado)
        {
            return _context.Set<Liame>()
                .Select(po => new Liame
                {
                    Liamestamp = po.Liamestamp,
                    Para = po.Para,
                    Assunto = po.Assunto,
                    Userno = encryptionHelper.DecryptText(po.Userno, po.Keystamp),
                    Corpo = encryptionHelper.DecryptText(po.Corpo, po.Keystamp),
                    Processado = po.Processado,
                    Ousrdata = po.Ousrdata,
                    Keystamp = po.Keystamp
                })
                .Where(po => po.Processado == processado)
                .ToList();
        }

        public List<UWspayments> GetWspayments(string batchid)
        {
            return _context.Set<UWspayments>()
                .Where(po => po.Batchid == batchid)
                .ToList();
        }

        public UWspayments GetWspaymentsByDestino(string batchid, string destino)
        {
            return _context.Set<UWspayments>()
                .Where(po => po.Batchid == batchid
                        && po.Oristamp == destino
                        )
                .FirstOrDefault();
        }


        public UWspayments GetWspaymentsByStamp(string stamp)
        {
            return _context.Set<UWspayments>()
                .Where(po => po.UWspaymentsstamp == stamp)
                .FirstOrDefault();
        }

        /*
        public Ft2 GetFt2(string ft2stamp)
        {
            return _context.Set<Ft2>().
                FirstOrDefault(ft => ft.Ft2stamp == ft2stamp);
        }

        */

    }
}

//PHCRepository