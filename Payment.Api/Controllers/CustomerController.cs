using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Payment.Api.Services;

namespace Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CustomerController : Controller
    {
        private readonly IPaymentService _paymentService;

        public CustomerController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetAllPayments(Guid customerId)
        {
            var result = await _paymentService.GetAllPayments(customerId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
