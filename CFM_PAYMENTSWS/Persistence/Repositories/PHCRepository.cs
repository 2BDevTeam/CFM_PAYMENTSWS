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

        public async Task<Ft?> GetFtByRef(string no)
        {
            var queryFT = from ft in _context.Set<Ft>()
                          join ft2 in _context.Set<Ft2>() on ft.Ftstamp equals ft2.Ft2stamp
                          where ft2.URefps2 == no.Trim()
                          select ft;

            return await queryFT.FirstOrDefaultAsync();
        }

        public async Task<Cl> getClienteByNo(decimal no)
        {
            return await _context.Set<Cl>().Where(cl => cl.No == no).FirstOrDefaultAsync();
        }
        public async Task<List<Cc>> getContaCorrenteByStamp(string stamp)
        {
            return await _context.Set<Cc>()
                .Where(cc => cc.Ccstamp == stamp).ToListAsync();
        }

        public decimal getMaxRecibo()
        {
            var config = getConfiguracaoRecibo();

            var rno = _context.Set<Re>()
                                .Where(re => re.Ndoc == config.Ndoc && re.Reano == DateTime.Now.Year)
                                .Select(re => re.Rno)
                                .ToList()
                                .DefaultIfEmpty(0)
                                .Max() + 1;

            return rno;
        }

        public Tsre getConfiguracaoRecibo()
        {
            return _context.Set<Tsre>().Where(tsre => tsre.UOnlinep == true).FirstOrDefault();
        }

        public Bl getBlByBancagr(string bancagroup)
        {
            return _context.Set<Bl>()
                .Where(bl => bl.UBancagr == bancagroup)
                .FirstOrDefault();
        }

        public void addRecibo(Re recibocc)
        {
            _context.Set<Re>().Add(recibocc);
        }

        public void addLinhasRecibo(Rl linhasRecibo)
        {
            _context.Set<Rl>().Add(linhasRecibo);
        }

        public void addTitulos(Rech titulo)
        {
            _context.Set<Rech>().Add(titulo);
        }

        public string getMoeda()
        {
            return _context.Set<Para1>().Where(para1 => para1.Descricao == "ge_pte").FirstOrDefault().Valor;

        }





    }
}

//PHCRepository