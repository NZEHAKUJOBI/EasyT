namespace PaymentController
{
    using backend.API.DTO.Request;
    using backend.API.Entities;
    using backend.API.DTO.Response;
    using Microsoft.AspNetCore.Mvc;
    using API.Interface;

    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public PaymentController(IJwtService jwtService)
        {
            _jwtService = jwtService;

        }

        [HttpPost("process")]
        public IActionResult ProcessPayment([FromBody] PaymentResponse paymentResponse)
        {
            if (paymentResponse == null)
            {
                return BadRequest("Invalid payment response.");
            }

            // Here you would typically process the payment and return a response.
            return Ok(paymentResponse);
        }
    }
}