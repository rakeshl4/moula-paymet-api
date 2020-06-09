using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Payment.Api.Models;
using Payment.Api.Services;

namespace Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create/{customerId}")]
        public async Task<IActionResult> CreatePayment
            ([FromBody]Models.Payment paymentRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _paymentService.CreatePayment(paymentRequest);
            return Ok(result);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelPayment
            ([FromBody]CancelPaymentRequest data)
        {
            var result = await _paymentService.CancelPayment
                (data.TransactionId, data.Comments);
            if (result == null)
                return NotFound();
            else return Ok();
        }

        [HttpPost("approve")]
        public async Task<IActionResult> ApprovePayment([FromBody]ApprovePaymentRequest data)
        {
            var result = await _paymentService.ApprovePayment(data.TransactionId);
            if (result == null)
                return NotFound();
            else return Ok();
        }
    }
}