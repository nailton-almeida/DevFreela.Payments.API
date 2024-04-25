using DevFreela.Payments.API.Model;

namespace DevFreela.Payments.API.Interfaces;

public interface IPaymentsService
{
    Task<bool> CheckPaymentIsCompletedAsync(PaymentInput payment);
}
