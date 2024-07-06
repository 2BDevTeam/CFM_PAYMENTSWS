//using Microsoft.Data.SqlClient.Server;
using LENMEDWS.Domains.Interface;
using LENMEDWS.Domains.Interfaces;
using LENMEDWS.Domains.Models;
using LENMEDWS.DTOs;
using LENMEDWS.Helper;
using LENMEDWS.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;

namespace LENMEDWS.Services
{
    public class PHCService : IPHCService
    {
        private readonly LogHelper logHelper = new LogHelper();


        //private readonly IPHCRepository _PHCRespository;
        private readonly IPHCRepository<AppDbContext> _phcRepository;
        private readonly IGenericRepository<AppDbContext> _genericRepository;

        public PHCService(IPHCRepository<AppDbContext> phcRepository, IGenericRepository<AppDbContext> genericRepository)
        {
            _phcRepository = phcRepository;
            _genericRepository = genericRepository;
        }


        public PHCService()
        {

        }



        public async Task<ResponseDTO> GetGLTransactions(string inicialDate, string finDate, int? pageIndex, int? pageSize)
        {
            try
            {
                var responseID = logHelper.generateResponseID();

                var e1 = _phcRepository.GetE1ByNomecomp("SEDE");
                var para1 = _phcRepository.GetPara1ByDescricao("mc_dataf");

                DateTime mcDataf = DateTime.ParseExact(para1.Valor.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);


                DateTime iDate = DateTime.ParseExact(inicialDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime finalDate = DateTime.ParseExact(finDate, "yyyyMMdd", CultureInfo.InvariantCulture);

                var glt = _phcRepository.GetGLTransactions(iDate, finalDate, e1.Ncont, mcDataf, pageIndex, pageSize);

                if (glt.Count == 0)
                {
                    throw new Exception("No data.");
                }


                var records = glt.Count;

                int pageI = 1;
                var pageS = glt.Count;
                int recordsPPage = glt.Count;

                if (pageIndex.HasValue && pageSize.HasValue)
                {
                    records = _phcRepository.GetGLTransactionsCount(iDate, finalDate);
                    pageI = pageIndex.Value;
                    pageS = pageSize.Value;
                }

                return new ResponseDTO(new HeaderDTO(records, pageI, pageS, recordsPPage)
                        , new ResponseCodesDTO("0000", "Success.", responseID), glt, null);
            }

            catch (FormatException)
            {
                return new ResponseDTO(new HeaderDTO(0, 0, 0, 0)
                        , new ResponseCodesDTO("0404", "Formatação da data inválida. Por favor, utilize 'yyyyMMdd'.", 0), null, null);
            }
            catch (Exception ex)
            {
                var errorDTO = new ErrorDTO { message = ex?.Message, stack = ex?.StackTrace?.ToString(), inner = ex?.InnerException?.ToString() + "  " };
                Debug.Print($"GetGLTransactions ERROR {errorDTO}");
                var finalResponse = new ResponseDTO(new ResponseCodesDTO("0007", "Error", logHelper.generateResponseID()), errorDTO.ToString(), null);
                logHelper.generateResponseLogJB(finalResponse, logHelper.generateResponseID().ToString(), "GetGLTransactions", errorDTO?.ToString());

                return new ResponseDTO(new HeaderDTO(0, 0, 0, 0)
                        , new ResponseCodesDTO("0404", errorDTO.message, 0), null, null);
            }
        }



    }

}
