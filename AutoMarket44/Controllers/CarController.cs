using AutoMarket44.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoMarket44.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : Controller
    {
        private readonly ICarService carService;

        public CarController(ICarService service)
        {
            carService = service;
        }
        [HttpGet]
        public IActionResult GetCars()
        {
            var response = carService.GetCars();
            if (response.StatusCode == AutoMarket44.Domain.Enum.StatusCode.OK)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Description);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await carService.DeleteCar(id);
            if (response.StatusCode == AutoMarket44.Domain.Enum.StatusCode.OK)
            {
                return Ok(response.Description);
            }
            return BadRequest(response.Description);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Save(int id)
        {
            if (id == 0)
                return Ok();

            var response = await carService.GetCar(id);
            if (response.StatusCode == AutoMarket44.Domain.Enum.StatusCode.OK)
            {
                return Ok(response.Data);
            }

            ModelState.AddModelError("", response.Description);
            return Ok();
        }

    }
}

