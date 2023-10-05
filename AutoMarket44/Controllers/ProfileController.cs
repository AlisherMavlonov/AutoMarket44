using AutoMarket44.Domain.ViewModels.Profile;
using AutoMarket44.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoMarket44.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        private readonly IProfileService profileService;

        public ProfileController(IProfileService service)
        {
            profileService = service;
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProfileViewModel model)
        {
            ModelState.Remove("Id");
            ModelState.Remove("UserName");
            if (ModelState.IsValid)
            {
                var response = await profileService.Save(model);
                if (response.StatusCode == AutoMarket44.Domain.Enum.StatusCode.OK)
                {
                    return Json(new { description = response.Description });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        public async Task<IActionResult> Detail()
        {
            var userName = User.Identity.Name;
            var response = await profileService.GetProfile(userName);
            if (response.StatusCode == AutoMarket44.Domain.Enum.StatusCode.OK)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Description);
        }
    }
}