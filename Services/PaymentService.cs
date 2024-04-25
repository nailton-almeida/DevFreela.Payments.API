using DevFreela.Payments.API.Interfaces;
using DevFreela.Payments.API.Model;

namespace DevFreela.Payments.API.Services;

public class PaymentService : IPaymentsService
{

    public async Task<bool> CheckPaymentIsCompletedAsync(PaymentInput payment)
    {
        return await Task.FromResult(true);
    }
}
