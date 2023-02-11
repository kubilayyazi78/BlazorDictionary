using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorDictionary.Api.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    // public Guid? UserId => new(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

    public Guid? UserId => new Guid("39E51C47-C176-4624-8F9E-FFE799BA27E0");
    
}