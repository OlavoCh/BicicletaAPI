using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bicicleta.Data;
using Microsoft.Extensions.Options;
using Mysqlx;

namespace Bicicleta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetIsActive : ControllerBase
    {

        [HttpGet("InGame")]
        public IActionResult ReturnActive()
        {
            if (Values.TextString == "c")
            {
                return Ok(new { isActive = Values.TextString });
            }
            else if (Values.TextString == "d")
            {
                return Ok(new { isActive = Values.TextString });
            }
            else
            {
                return BadRequest(new { Error = "erro" });
            }
        }

    }
}
