using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LENMEDWS.Domains.Interfaces;
using LENMEDWS.DTOs;

namespace LENMEDWS.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api")]
    [ApiController]
    public class LENMEDController : ControllerBase
    {
        IPHCService _phcService;

        public LENMEDController(IPHCService phcService)
        {
            _phcService = phcService;
        }

        [HttpGet]
        //[Route("gltransactions")]
        [Route("gltransactions/{initialDate}/{finalDate}")]
        public async Task<ActionResult<ResponseDTO>> GetGLTransactions(string initialDate, string finalDate, int? pageIndex = null, int? pageSize = null)
        {
            var response = await _phcService.GetGLTransactions(initialDate, finalDate, pageIndex, pageSize);
            return Ok(response);
        }


        /*

        [HttpGet]
        [Route("registarCliente")]
        public async Task<ActionResult<ResponseDTO>> RegistarCliente(int id)
        {
            var response = _koboService.RegistarCliente(id);
            return Ok(response);
        }
        */
    }
}

