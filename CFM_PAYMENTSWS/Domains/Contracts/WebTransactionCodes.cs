using CFM_PAYMENTSWS.DTOs;

namespace CFM_PAYMENTSWS.Domains.Contracts
{
     public class WebTransactionCodes
    {
        public static ResponseCodesDTO SUCCESS = new ResponseCodesDTO("0000", "Success");
        public static ResponseCodesDTO SUCCESS_PT = new ResponseCodesDTO("0000", "Recebido com sucesso");
        public static ResponseCodesDTO ERROR = new ResponseCodesDTO("0404", "Error: {0}");
        public static ResponseCodesDTO INCORRECTHTTP = new ResponseCodesDTO("0001", "Incorrect HTTP method");
        public static ResponseCodesDTO PENDINGBATCH = new ResponseCodesDTO("1001", "Pending Batch");
        public static ResponseCodesDTO PENDINGPAYMENT_PT = new ResponseCodesDTO("1001", "Pagamento pendente");
        public static ResponseCodesDTO PENDINGPAYMENT = new ResponseCodesDTO("1002", "Pending Payment");
        public static ResponseCodesDTO TRANSACTIONNOTFOUND_PT = new ResponseCodesDTO("1002", "Pagamento não encontrado");

        public static ResponseCodesDTO INVALIDJSON = new ResponseCodesDTO("0002", "Invalid JSON");
        public static ResponseCodesDTO INCORRECTAPIKEY = new ResponseCodesDTO("0003", "Incorrect API Key");
        public static ResponseCodesDTO APIKEYNOTFOUND = new ResponseCodesDTO("0004", "Api Key not provided");
        public static ResponseCodesDTO INVALIDREFERENCE = new ResponseCodesDTO("0005", "Invalid Reference {0} for Payment {1}");
        public static ResponseCodesDTO INVALIDREFERENCE_PT = new ResponseCodesDTO("0005", "A referência {0} do pagamento {1} é inválida");
        public static ResponseCodesDTO DUPLICATEDPAYMENT = new ResponseCodesDTO("0006", "Duplicated payment for {0}");
        public static ResponseCodesDTO DUPLICATEDPAYMENT_PT = new ResponseCodesDTO("0006", "Pagamento {0} duplicado");
        public static ResponseCodesDTO INTERNALERROR = new ResponseCodesDTO("0007", "Internal Error");
        public static ResponseCodesDTO INVALIDAMOUNT = new ResponseCodesDTO("0008", "Invalid Amount Used");
        public static ResponseCodesDTO MUSTPROVIDEREQUESTID = new ResponseCodesDTO("0009", "Request Id not provided");
        public static ResponseCodesDTO EXTERNALERROR = new ResponseCodesDTO("0010", "External error");
        public static ResponseCodesDTO PREVALIDATIONERROR = new ResponseCodesDTO("0011", "Pre-validation Error");
        public static ResponseCodesDTO USERALREADYEXISTS = new ResponseCodesDTO("0010", "User already exists!");
        public static ResponseCodesDTO USERCREATIONFAILED = new ResponseCodesDTO("0011", "User creation failed! Please check user details and try again.");
        public static ResponseCodesDTO SUCCESSPAYMENT = new ResponseCodesDTO("1000", "Payment successfully processed");
        public static ResponseCodesDTO SUCCESSPAYMENT_PT = new ResponseCodesDTO("1000", "Pagamento processado com sucesso");

    }
}
