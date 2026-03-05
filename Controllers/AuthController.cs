using HomeBudgetAPI.Data;
using HomeBudgetAPI.DTOs.Auth;
using HomeBudgetAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HomeBudgetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterUserAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Neispravan zahtjev.");

            var result = await _authService.LoginUserAsync(request);

            if (!result.Success)
                return Unauthorized(new { result.Message });

            return Ok(result);
        }

    }
}
