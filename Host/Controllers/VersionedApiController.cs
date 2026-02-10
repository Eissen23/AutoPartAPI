using Asp.Versioning;

namespace Host.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class VersionedApiController : BaseController
{
}