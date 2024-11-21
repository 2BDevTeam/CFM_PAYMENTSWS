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
        public async Task<ActionResult<ResponseDTO>> GetGLTransactions(PaymentCheckedDTO pagamentos)
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
}

