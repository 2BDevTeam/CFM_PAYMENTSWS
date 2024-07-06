using LENMEDWS.Extensions;
using System;
using System.Collections.Generic;

namespace LENMEDWS.Domains.Models
{
    public partial class ApiLogs
    {
        public string UApilogstamp { get; set; } =  25.UseThisSizeForStamp();
        public string? RequestId { get; set; }
        public DateTime? Data { get; set; }
        public string? Code { get; set; }
        public string? Content { get; set; }
        public string? ResponseDesc { get; set; }
        public string? Operation { get; set; }
        public string? ResponseText { get; set; }
    }
}
