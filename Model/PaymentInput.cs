namespace DevFreela.Payments.API.Model
{
    public class PaymentInput
    {
        public Guid IdProject { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }

        public string CardCVV { get; set; }

        public string ValidateAt { get; set; }
    }
}