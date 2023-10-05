using AutoMarket44.Domain.Extensions;
using AutoMarket44.Domain.ViewModels.User;
using AutoMarket44.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoMarket44.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService service;
        public UserController(IUserService userService)
        {
            service = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await service.GetUsers();
            if (response.StatusCode == AutoMarket44.Domain.Enum.StatusCode.OK)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Description);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var response = await service.DeleteUser(id);
            if (response.StatusCode == AutoMarket44.Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetAll");
            }
            return BadRequest(response.Description);

        }

        [HttpPost]
        public async Task<IActionResult> Save(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var response = await service.Create(user);
                if (response.StatusCode == AutoMarket44.Domain.Enum.StatusCode.OK)
                {
                    return Json(new { description = response.Description });
                }
                return BadRequest(new { errorMesage = response.Description });
            }
            var errorMessage = ModelState.Values
                .SelectMany(x => x.Errors.Select(x => x.ErrorMessage)).ToList().Join();

            return StatusCode(StatusCodes.Status500InternalServerError, new { errorMessage });
        }
    }
}
