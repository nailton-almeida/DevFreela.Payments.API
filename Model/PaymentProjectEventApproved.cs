namespace DevFreela.Payments.API.Model
{
    public class PaymentProjectEventApproved
    {
        public PaymentProjectEventApproved(Guid IdProject)
        {
            this.IdProject = IdProject;
        }

        public Guid IdProject { get; set; }
    }
}