using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bicicleta.Data;

namespace Bicicleta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetIsActive : ControllerBase
    {
        private static bool _IsCycling { get; set; }
        private static bool _isActive;
        [HttpPut("InGame")]
       public IActionResult UpdateStatus([FromBody] bool active)
        {
            _isActive = active;

            return Ok();
        }

        [HttpGet("InGame")]
        public IActionResult ReturnActive() {
            return Ok(new {isActive = _isActive });
        }

        [HttpPost("InCycling")]
        public IActionResult Cycling([FromBody] bool Cycling)
        {
            _IsCycling = Cycling;
            return Ok();
        }

        [HttpGet("InCycling")]
        public IActionResult GetCycling()
        {
            return Ok(new {isCycling = _IsCycling });
        }
    }
}
