using DevFreela.Payments.API.Interfaces;
using DevFreela.Payments.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.Payments.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentsService _paymentsService;
        public PaymentsController(IPaymentsService paymentsService)
        {
            _paymentsService =  paymentsService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentInput payment)
        {
            var result = await _paymentsService.CheckPaymentIsCompletedAsync(payment);
            if (result == false)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
