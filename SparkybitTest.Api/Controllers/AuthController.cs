using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkybitTest.Api.Dto;
using SparkybitTest.Api.Dto.Validations;
using SparkybitTest.Api.Services;

namespace SparkybitTest.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    // TODO Hardcode user and password
    private const string HardCodeLogin = "test";
    private const string HardCodePassword = "1234";

    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService)
    {
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var validator = new LoginRequestDtoValidation();

        var validationResult = validator.Validate(loginRequestDto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        if (loginRequestDto.Login == HardCodeLogin && loginRequestDto.Password == HardCodePassword)
        {
            var token = _jwtService.GenerateJwt(Guid.NewGuid());

            return Ok(new LoginResponseDto(token));
        }

        return Unauthorized();
    }
}