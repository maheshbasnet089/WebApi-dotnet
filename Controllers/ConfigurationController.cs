
using Microsoft.AspNetCore.Mvc;

namespace webAPIDevelopment.Controllers; 

[ApiController]
[Route("api/[controller]")]

public class ConfigurationController(IConfiguration configuration): ControllerBase{
    [HttpGet]
    [Route("secret")]
    public ActionResult GetMyKey(){
        var myKey = configuration["Secret"]; 
        return Ok(myKey);
    }
}
