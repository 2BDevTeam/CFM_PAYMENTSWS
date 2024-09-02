using Newtonsoft.Json;

namespace CFM_PAYMENTSWS.DTOs
{
    public class PropertyResult
    {
        public object PropertyValue { get; set; }
        public Type PropertyType { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
