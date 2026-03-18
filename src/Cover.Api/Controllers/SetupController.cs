using Microsoft.AspNetCore.Mvc;
using Cover.Api.Services;
using Cover.Shared.DTOs;

namespace Cover.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SetupController : ControllerBase
{
    private readonly IUserService _userService;

    public SetupController(IUserService userService) => _userService = userService;

    [HttpGet("status")]
    public async Task<SetupStatusDto> GetStatus()
    {
        var isComplete = await _userService.IsSetupCompleteAsync();
        return new SetupStatusDto(isComplete);
    }

    [HttpPost]
    public async Task<ActionResult<List<UserDto>>> Setup(SetupRequest request)
    {
        if (await _userService.IsSetupCompleteAsync())
            return BadRequest("Setup already completed");

        if (string.IsNullOrWhiteSpace(request.Name1) || string.IsNullOrWhiteSpace(request.Name2))
            return BadRequest("Both names are required");

        var users = await _userService.CreateUsersAsync(request.Name1.Trim(), request.Name2.Trim());
        return Ok(users);
    }
}
