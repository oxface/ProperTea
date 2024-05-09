using Microsoft.AspNetCore.Mvc;

namespace ProperTea.Property.Api;

[ApiController]
[Route("[controller]")]
public class PropertyController : ControllerBase
{
    [HttpGet]
    public string Hello() => "yo";
}
