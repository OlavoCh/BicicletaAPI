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
            public string? Values { get; set; }
        }

        [HttpPost]
        public IActionResult GetScore([FromBody] ScoreModel data)
        {
            char[] delimiter = [','];
            string? text = data.Values;

            string[] finaltext = text.Split(delimiter);


            int num = int.Parse(finaltext[0]);
            string str = finaltext[1];


            Values.Score = num;
            Values.TextString = str;
            
            return Ok(new { message = "Score enviado com sucesso." });
        }

        [HttpGet]
        public IActionResult ShowScore()
        {
            return Ok(new { score = Values.Score });
        }
    }


}
