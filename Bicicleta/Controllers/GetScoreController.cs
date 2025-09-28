using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bicicleta.Data;

namespace Bicicleta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetScoreController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] int score)
        {   
            Values.score = score;
            return Ok(Values.score);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Values.score);
        }
    }


}
