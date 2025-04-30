using ChatRoomServer.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedContracts;

namespace ChatRoomServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(ILogger<UsersController> logger, IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<UserInfo>> Get()
    {
        logger.LogDebug("Get users called.");

        return await mediator.Send(new GetUsersQuery());
    }

    [HttpPost]
    public async Task<IActionResult> LoginUser([FromQuery] string userName)
    {
        logger.LogDebug("Login user called with user name: {UserName}", userName);

        if (userName == null)
        {
            return BadRequest("User name cannot be null");
        }
        var userInfo = await mediator.Send(new LoginUserQuery(userName));
        return Ok(userInfo);
    }
}
