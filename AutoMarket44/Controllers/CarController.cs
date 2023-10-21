using AutoMarket44.Domain.ViewModels.Car;
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

        [HttpPost]
        [Route("/AddCar")]
        public async Task<IActionResult> Create(CarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await carService.Create(model);
                if (response.StatusCode == AutoMarket44.Domain.Enum.StatusCode.OK)
                {
                    return Ok(response.Data);
                }
                
                return BadRequest(response.Description);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("/GetCars")]
        public async Task<IActionResult> GetCars()
        {
            var response = await carService.GetCars();
            if (response.StatusCode == AutoMarket44.Domain.Enum.StatusCode.OK)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Description);
        }

        [HttpDelete]
        [Route("/DeleteCar")]
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

        [HttpGet]
        [Route("/GetCar")]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
                return Ok();

            var response = await carService.GetCar(id);
            if (response.StatusCode == AutoMarket44.Domain.Enum.StatusCode.OK)
            {
                return Ok(response.Data);
            }

            ModelState.AddModelError("", response.Description);
            return Ok(response.Description);
        }

        [HttpPut]
        [Route("/UpdateCar")]
        public async Task<IActionResult> UpdateCar(CarViewModel model)
        {

            if (ModelState.IsValid)
            {
                var response = await carService.Edit( model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return Ok(response.Description);
                }

                return BadRequest(response.Description);
            }
            return BadRequest("не валидный данный");
        }
    }
}

