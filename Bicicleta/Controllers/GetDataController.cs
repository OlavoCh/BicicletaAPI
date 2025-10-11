using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json.Serialization;
using Bicicleta.Data;

namespace Bicicleta.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class GetDataController : ControllerBase
    {
        private readonly string connectionString = "server=localhost;database=bicicleta;user=root;password=''";
        public class PlayerScore
        {
            public string? Name { get; set; }
            public int Score { get; set; }
            public int Posicao { get; set; }
        }

        [HttpGet]
        public IActionResult Get()
        {

            var scores = new List<PlayerScore>();
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT name, score, ROW_NUMBER() OVER (ORDER BY score DESC) AS posicao FROM scores LIMIT 10;";
            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                scores.Add(new PlayerScore
                {
                    Name = reader.GetString(0),
                    Score = reader.GetInt32(1),
                    Posicao = reader.GetInt32(2)
                });
            }

            connection.Close();
            return Ok(scores);

        }
    }
}
