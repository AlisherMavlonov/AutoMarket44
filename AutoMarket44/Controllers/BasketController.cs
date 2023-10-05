using AutoMarket44.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoMarket44.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : Controller
    {
        private readonly IBasketService basketService;

        public BasketController(IBasketService service)
        {
            basketService = service;
        }

        [HttpGet]
        public async Task<IActionResult> Detail()
        {
            var response = await basketService.GetItems(User.Identity.Name);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Description);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(long id)
        {
            var response = await basketService.GetItem(User.Identity.Name, id);
            if (response.StatusCode == AutoMarket44.Domain.Enum.StatusCode.OK)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Description);
        }
    }
}