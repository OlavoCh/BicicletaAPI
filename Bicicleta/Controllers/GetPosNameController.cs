using Bicicleta.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Bicicleta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetPosNameController : ControllerBase
    {
        private readonly string connectionString = "server=localhost;database=bicicleta;user=root;password=''";
        public class PlayerScore
        {
            public string? Nome { get; set; }
            public int Pontos { get; set; }
            public int Posicao { get; set; }
        }


        [HttpGet]
        public IActionResult GetPlayerRanking()
        {
            var scores = new List<PlayerScore>();

            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = @"
        SELECT name, score, posicao 
        FROM (
            SELECT name, score, ROW_NUMBER() OVER (ORDER BY score DESC) AS posicao
            FROM scores
        ) AS position
        WHERE name = @name";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", Values.Name);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                scores.Add(new PlayerScore
                {
                    Nome = reader.GetString(reader.GetOrdinal("name")),    // name no banco
                    Pontos = reader.GetInt32(reader.GetOrdinal("score")), // score no banco
                    Posicao = reader.GetInt32(reader.GetOrdinal("posicao"))
                });
            }

            if (scores.Count == 0)
                return NotFound(); // jogador não encontrado

            return Ok(scores[0]); // retorna só o jogador filtrado
        }

    }
}
