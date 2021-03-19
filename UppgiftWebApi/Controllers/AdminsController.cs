using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedLibrary;
using UppgiftWebApi.Services;

namespace UppgiftWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminsController(IAdminService adminService)
        {
            _adminService = adminService;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            if (await _adminService.CreateAdminAsync(model))
                return new OkResult();

            return new BadRequestResult();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LogIn model)
        {
            var response = await _adminService.LogInAsync(model.Email, model.Password);
            if (response.Succeeded)
                return new OkObjectResult(response.Result);
                return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAdmins()
        {
            if (_adminService.ValidateAccessRights(IdentityRequestAdminData()))
                return new OkObjectResult(await _adminService.GetAdminAsync());
            return new UnauthorizedResult();
        }

        private RequestAdmin IdentityRequestAdminData()
        {
            return new RequestAdmin
            {
                AdminId = int.Parse(HttpContext.User.FindFirst("AdminId").Value),
                AccessToken = Request.Headers["Authorization"].ToString().Split(" ")[1]
            };
        }

    }
}
