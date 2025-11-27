using CFM_PAYMENTSWS.DTOs;
using System.Runtime.Serialization;

namespace CFM_PAYMENTSWS.Domains.Exceptions
{
    [Serializable]
    public class GeneralException:Exception
    {
        public ResponseDTO responseDTO { get; set; }
        public GeneralException()
        {
        }

        public GeneralException(ResponseDTO responseDTO)
        {
            this.responseDTO = responseDTO;
        }

        public GeneralException(string? message) : base(message)
        {
        }

        public GeneralException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected GeneralException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
