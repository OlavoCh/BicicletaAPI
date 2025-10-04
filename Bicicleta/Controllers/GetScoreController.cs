using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bicicleta.Data;

namespace Bicicleta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetScoreController : ControllerBase
    {

        public class ScoreModel
        {
            public int Score { get; set; }
        }

        [HttpPost]
        public IActionResult GetScore([FromBody] ScoreModel data)
        {   
            Values.Score = data.Score;
            return Ok(data.Score);
        }

        [HttpGet]
        public IActionResult ShowScore()
        {
            return Ok(new {score = Values.Score});
        }
    }


}
