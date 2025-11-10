using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CFM_PAYMENTSWS.Providers.FCB.DTOs
{
    public class FcbResponseDTO
    {
        [JsonProperty("batchId")]
        public string BatchId { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("processingDate")]
        public string ProcessingDate { get; set; } = string.Empty;

        [JsonProperty("initgPtyCode")]
        public string InitgPtyCode { get; set; } = string.Empty;

        [JsonProperty("batchBooking")]
        public string BatchBooking { get; set; } = string.Empty;

        [JsonProperty("debitAccount")]
        public string DebitAccount { get; set; } = string.Empty;

        [JsonProperty("statusCode")]
        public string StatusCode { get; set; } = string.Empty;

        [JsonProperty("statusDescription")]
        public string StatusDescription { get; set; } = string.Empty;

        [JsonProperty("paymentRecordsStatus")]
        public List<FcbPaymentRecordStatusDTO> PaymentRecordsStatus { get; set; } = new();

        [JsonProperty("resultInfo")]
        public FcbResultInfoDTO ResultInfo { get; set; } = new();

        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class FcbPaymentRecordStatusDTO
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; } = string.Empty;

        [JsonProperty("creditAccount")]
        public string CreditAccount { get; set; } = string.Empty;

        [JsonProperty("beneficiaryName")]
        public string BeneficiaryName { get; set; } = string.Empty;

        [JsonProperty("transactionDescription")]
        public string TransactionDescription { get; set; } = string.Empty;

        [JsonProperty("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("beneficiaryEmail")]
        public string BeneficiaryEmail { get; set; } = string.Empty;

        [JsonProperty("statusCode")]
        public string StatusCode { get; set; } = string.Empty;

        [JsonProperty("statusDescription")]
        public string StatusDescription { get; set; } = string.Empty;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class FcbResultInfoDTO
    {
        [JsonProperty("rcode")]
        public int RCode { get; set; }

        [JsonProperty("messages")]
        [JsonConverter(typeof(FcbMessagesConverter))]
        public List<FcbResultMessageDTO> Messages { get; set; } = new();

        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class FcbResultMessageDTO
    {
        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken>? AdditionalData { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class FcbMessagesConverter : JsonConverter<List<FcbResultMessageDTO>>
    {
        public override List<FcbResultMessageDTO> ReadJson(JsonReader reader, Type objectType, List<FcbResultMessageDTO> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var messages = new List<FcbResultMessageDTO>();
            if (reader.TokenType == JsonToken.Null)
            {
                return messages;
            }

            var token = JToken.Load(reader);
            switch (token.Type)
            {
                case JTokenType.Array:
                    foreach (var item in token.Children())
                    {
                        if (item.Type == JTokenType.Object)
                        {
                            var dto = item.ToObject<FcbResultMessageDTO>(serializer);
                            if (dto != null)
                            {
                                messages.Add(dto);
                            }
                        }
                        else if (item.Type == JTokenType.String)
                        {
                            messages.Add(new FcbResultMessageDTO { Description = item.ToString() });
                        }
                    }
                    break;
                case JTokenType.Object:
                    var message = token.ToObject<FcbResultMessageDTO>(serializer);
                    if (message != null)
                    {
                        messages.Add(message);
                    }
                    break;
                case JTokenType.String:
                    messages.Add(new FcbResultMessageDTO { Description = token.ToString() });
                    break;
            }

            return messages;
        }

        public override void WriteJson(JsonWriter writer, List<FcbResultMessageDTO> value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
