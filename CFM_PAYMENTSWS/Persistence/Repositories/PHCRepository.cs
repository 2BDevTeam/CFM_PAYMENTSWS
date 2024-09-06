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
                    Keystamp=po.Keystamp
                })
                .Where(po => po.Processado == processado)
                .ToList();
        }


        public string SendEmail(string email, string subject, string body)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json");

            var config = configuration.Build();
            var connectionString = config.GetConnectionString("ConnStrE14");
            Debug.Print($"connectionString {connectionString}");


            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("EXEC msdb.dbo.sp_send_dbmail @profile_name = 'CFM-NoReply', @recipients = @EmailAddress, @subject = @Subject, @body = @FinalBody, @body_format = 'HTML';", connection))
                {
                    command.Parameters.AddWithValue("@EmailAddress", email);
                    command.Parameters.AddWithValue("@Subject", subject);
                    command.Parameters.AddWithValue("@FinalBody", body);

                    command.ExecuteNonQuery();
                }

                using (var checkCommand = new SqlCommand(
                    "SELECT TOP 1 mailitem_id, recipients, subject, send_request_date, sent_status " +
                    "FROM msdb.dbo.sysmail_allitems " +
                    "WHERE recipients = @EmailAddress " +
                    "ORDER BY send_request_date DESC", connection))
                {
                    checkCommand.Parameters.AddWithValue("@EmailAddress", email);
                    using (var reader = checkCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var response = new
                            {
                                MailId = reader["mailitem_id"],
                                Recipients = reader["recipients"],
                                Subject = reader["subject"],
                                SendRequestDate = reader["send_request_date"],
                                Status = reader["sent_status"]
                            };

                            return JsonConvert.SerializeObject(response);
                        }
                        else
                        {
                            return "failed";
                        }
                    }
                }

            }
        }

        /*
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