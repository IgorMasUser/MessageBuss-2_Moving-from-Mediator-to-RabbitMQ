using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sample.Contracts;

namespace MassTransit_Starting_with_Mediator.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class OrderController : ControllerBase
    {

        private readonly ILogger<OrderController> _logger;
        private readonly IRequestClient<SubmitOrder> _submitOrderRequestClient;

        public OrderController(ILogger<OrderController> logger, IRequestClient<SubmitOrder> submitOrderRequestClient)
        {
            _logger = logger;
            _submitOrderRequestClient = submitOrderRequestClient;
        }


        [HttpPost]
        public async Task<IActionResult> Post(Guid id, string customerNumber)
        {
            var (accepted, rejected) = await _submitOrderRequestClient.GetResponse<OrderSubmissionAccepted, OrderSubmissionRejected>(new
            {
                OrderId = id,
                InVar.Timestamp,
                CustomerNumber = customerNumber
            });
            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;
                return Ok(response);
            }

            else
            {
                var response = await rejected;
                return BadRequest(response.Message);
            }
        }
    }

}
