using AutoMarket44.Domain.ViewModels.Order;
using AutoMarket44.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoMarket44.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService service)
        {
            orderService = service;
        }

        [HttpGet]
        public IActionResult CreateOrder(long id)
        {
            var orderModel = new CreateOrderViewModel()
            {
                CarId = id,
                Login = User.Identity.Name,
                Quantity = 0
            };

            return Ok(orderModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await orderService.Create(model);
                if (response.StatusCode == AutoMarket44.Domain.Enum.StatusCode.OK)
                {
                    return Json(new { description = response.Description });
                };
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var response = await orderService.Delete(Id);
            if (response.StatusCode == AutoMarket44.Domain.Enum.StatusCode.OK)
            {
                return Ok(response.Description);
            }

            return Ok(response.Description);
        }



    }
}
