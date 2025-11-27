using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CFM_PAYMENTSWS.Domains.Interfaces;
using CFM_PAYMENTSWS.DTOs;

namespace CFM_PAYMENTSWS.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/payment")]
    [ApiController]
    public class CFM_PAYMENTSWSController : ControllerBase
    {
        IPaymentService _paymentService;

        public CFM_PAYMENTSWSController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpPost]
        [Route("UpdatePayments")]
        public async Task<ActionResult<ResponseDTO>> UpdatePayments(PaymentCheckedDTO pagamentos)
        {
            var response = await _paymentService.actualizarPagamentos(pagamentos);
            return Ok(response);
        }


        [HttpPost]
        [Route("EnviarPagamento")]
        public async Task<ActionResult<RespostaDTO>> ProcessarPagamentos(
            [FromBody] PaymentDetailsDTO paymentDetailsDTOs)
        {

            //paymentDetailsDTOs.Destino = "171717";
            paymentDetailsDTOs.Canal = 101;
            var response = await _paymentService.ProcessarPagamentos(paymentDetailsDTOs);

            if (response.Codigo != "0000")
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

    }


    //CFM_PAYMENTSWS V1 documentado a partir da Descrição técnica V1.61
    //[Authorize]
    [Route("api/payment/v1")]
    [ApiController]
    public class CFM_PAYMENTSWSV1Controller : ControllerBase
    {
        IPaymentService _paymentService;

        public CFM_PAYMENTSWSV1Controller(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpPost]
        [Route("updatePayments")]
        public async Task<ActionResult<ResponseSmallDTO>> UpdatePayments(PaymentCheckedDTO pagamentos)
        {
            var response = await _paymentService.actualizarPagamentos(pagamentos);

            ResponseSmallDTO responseSmallDTO = new()
            {
                code = response.response.cod,
                description = response.response.codDesc
            };

            return Ok(responseSmallDTO);
        }



        [HttpPost]
        [Route("enviarPagamentos")]
        public async Task<ActionResult<List<RespostaDTO>>> GetPayment([FromBody] List<PaymentDynamicDTO> payments)
        {
            var result = await _paymentService.InsertPayments(payments);
            return Ok(result);
        }


    }


}

