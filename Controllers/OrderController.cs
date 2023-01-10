using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebApi.Common;
using WebApi.Models.Order;
using WebApi.Resources;
using WebApi.Services.Interface;
using static WebApi.Common.Enums;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IStringLocalizer<Resource> localizer;
        private readonly ILogger<OrderController> logger;

        public OrderController(
            IOrderService orderService,
            IStringLocalizer<Resource> localizer,
            ILogger<OrderController> logger)
        {
            this.orderService = orderService;
            this.localizer = localizer;
            this.logger = logger;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAsync([FromQuery] int? id = null, [FromQuery] OrderStatus? status = null)
        {
            try
            {
                var result = await orderService.GetAllAsync(id, status);

                return Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);

                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateOrderModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Errors());
                }

                var result = await orderService.CreateAsync(model);

                return Ok(result);
            }
            catch (ArgumentException aex)
            {
                this.logger.LogError(aex, aex.Message);

                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);

                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] EditOrderModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Errors());
                }

                await orderService.EditAsync(model);

                return Ok();
            }
            catch (ArgumentException aex)
            {
                this.logger.LogError(aex, aex.Message);

                return BadRequest(localizer.GetString(aex.Message).Value);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);

                return StatusCode(500);
            }
        }
    }
}
