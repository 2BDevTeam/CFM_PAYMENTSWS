using AutoMapper;
using CFM_PAYMENTSWS.Domains.Contracts;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;
using Microsoft.AspNetCore.Routing.Constraints;
using CFM_PAYMENTSWS.Domains.Contracts;
using CFM_PAYMENTSWS.DTOs;
using CFM_PAYMENTSWS.Extensions;
using CFM_PAYMENTSWS.Providers.Nedbank.DTOs;
using System.Diagnostics;

namespace CFM_PAYMENTSWS.Mappers
{
    public class NedbankMapper
    {
        public ResponseDTO mapNedbankLoadPayments(NedbankResponseDTO nedbankResponseDTO)
        {

            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<NedbankResponseDTO, ResponseDTO>()
                .ForPath(dest => dest.response, act => act.MapFrom(src => getStatusCode(src)))
                .ForPath(dest => dest.Data, act => act.MapFrom(src => nedbankResponseDTO.ToString())
                ));
            var mapper = new Mapper(config);
            var responseDTO = mapper.Map<ResponseDTO>(nedbankResponseDTO);

            return responseDTO;
        }

        public CheckPaymentReportResponseDTO mapNedbankCheckPayment(NedBankCheckPaymentReportResponseDTO nedbankResponseDTO)
        {
            // ResponseDTO(ResponseCodesDTO response, object? data, object? content)
            Debug.Print($"CHECK DATA {nedbankResponseDTO.ToString()}");
            var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<NedBankCheckPaymentReportResponseDTO, CheckPaymentReportResponseDTO>()
               .ForPath(dest => dest.batchResponse, act => act.MapFrom(src => mapBatchResponse(src)))
               .ForPath(dest => dest.paymentRecordsResponse, o => o.MapFrom(src => src.PaymentRecordsStatus.Select(paymentRecord => new PaymentRecordResponseDTO
               {

                   transactionId = paymentRecord.TransactionId,
                   bankReference = paymentRecord.BankReference,
                   paymentRecordsResponse = new ResponseDTO(getPaymentRecordStatusCode(paymentRecord), paymentRecord.ToString(), null)

               })))
               );
            var mapper = new Mapper(config);
            var responseDTO = mapper.Map<CheckPaymentReportResponseDTO>(nedbankResponseDTO);

            return responseDTO;

        }

        public ResponseDTO mapBatchResponse(NedBankCheckPaymentReportResponseDTO checkPaymentReportResponseDTO)
        {
            var responseCodes = new ResponseCodesDTO();
            if (checkPaymentReportResponseDTO.StatusCode == "1000")
            {
                responseCodes = WebTransactionCodes.SUCCESS;
                return new ResponseDTO(responseCodes, checkPaymentReportResponseDTO.ToString(), null);
            }

            if (checkPaymentReportResponseDTO.StatusCode == "1001")
            {
                responseCodes = WebTransactionCodes.PENDINGBATCH;
                return new ResponseDTO(responseCodes, checkPaymentReportResponseDTO.ToString(), null);
            }

            if (int.TryParse(checkPaymentReportResponseDTO.StatusCode, out int number))
            {
                if (number >= 400 && number <= 499)
                {
                    responseCodes = new ResponseCodesDTO("0007", checkPaymentReportResponseDTO.StatusDescription);
                    return new ResponseDTO(responseCodes, checkPaymentReportResponseDTO.ToString(), null);
                }
                if (number >= 500 && number <= 599)
                {
                    responseCodes = new ResponseCodesDTO("0010", checkPaymentReportResponseDTO.StatusDescription);
                    return new ResponseDTO(responseCodes, checkPaymentReportResponseDTO.ToString(), null);
                }
            }


            responseCodes = new ResponseCodesDTO("0011", checkPaymentReportResponseDTO.StatusDescription);
            return new ResponseDTO(responseCodes, checkPaymentReportResponseDTO.ToString(), null);
        }

        public ResponseCodesDTO getPaymentRecordStatusCode(PaymentRecordsDTO paymentRecordsDTO)
        {

            if (paymentRecordsDTO.StatusCode == "0000")
                return WebTransactionCodes.SUCCESS;
            if (paymentRecordsDTO.StatusCode == "1000")
                return new ResponseCodesDTO("1000", paymentRecordsDTO.StatusDescription);
            if (paymentRecordsDTO.StatusCode == "1001")
                return WebTransactionCodes.PENDINGPAYMENT;
            if (int.TryParse(paymentRecordsDTO.StatusCode, out int number))
            {
                if (number >= 400 && number <= 499)
                    return new ResponseCodesDTO("0007", paymentRecordsDTO.StatusDescription);
                if (number >= 500 && number <= 599)
                    return new ResponseCodesDTO("0010", paymentRecordsDTO.StatusDescription);
            }


            return new ResponseCodesDTO("0011", paymentRecordsDTO.StatusDescription);

        }



        public ResponseCodesDTO getStatusCode(NedbankResponseDTO nedbankResponseDTO)
        {

            if (nedbankResponseDTO.StatusCode == "0000")
                return WebTransactionCodes.SUCCESS;
            if (int.TryParse(nedbankResponseDTO.StatusCode, out int number))
            {
                if (number >= 400 && number <= 499)
                    return new ResponseCodesDTO("0007", nedbankResponseDTO.StatusDescription);
                if (number >= 500 && number <= 599)
                    return new ResponseCodesDTO("0010", nedbankResponseDTO.StatusDescription);
            }


            return new ResponseCodesDTO("0011", nedbankResponseDTO.StatusDescription);

        }



    }
}
