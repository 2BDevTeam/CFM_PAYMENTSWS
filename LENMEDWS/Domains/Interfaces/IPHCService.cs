using LENMEDWS.DTOs;

namespace LENMEDWS.Domains.Interfaces
{
    public interface IPHCService
    {
        /*
        public ResponseDTO GetResult(string nome);
        public ResponseDTO RegistarCliente(int id);
        */
        public Task<ResponseDTO> GetGLTransactions(string inicialDate, string finDate, int? pageIndex, int? pageSize);

    }
}
