using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Host.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

}
