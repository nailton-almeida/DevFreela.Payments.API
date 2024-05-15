using System.Text.Json.Serialization;

namespace DevFreela.Payments.API.Model
{
    public class PaymentInput
    {
        public Guid IdProject { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }

        [JsonPropertyName("ValidateDate")]
        public string ValidateAt { get; set; }
    }
}