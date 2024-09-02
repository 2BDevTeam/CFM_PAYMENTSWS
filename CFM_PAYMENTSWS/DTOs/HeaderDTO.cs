using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.DTOs
{
    public class HeaderDTO
    {
        public HeaderDTO(int records,int pageIndex, int pageSize, int recordsPPage)
        {
            Records = records;
            PageIndex = pageIndex;
            PagesSize = pageSize;
            RecordsPPage = recordsPPage;
        }
        public int Records{ get; set; }
        public int PageIndex { get; set; }
        public int PagesSize { get; set; }
        public int RecordsPPage { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
