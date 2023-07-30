using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V2.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/users")]
public class UsersController : ControllerBase
{

    [HttpGet("{userid}")]
    public IActionResult Get(int userid)
    {
        return Ok();
    }

    [HttpPost]
    public ActionResult PostTodoItem([FromBody] UserModel userModel)
    {
        return Ok();
    }
}

[Serializable]
public class UserModel
{
    public string username { get; set; }
    public string password { get; set; }
}

[Serializable]
public class TodoItemDTO
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}
