using Bicicleta.Data; // Seus usings
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Bicicleta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetPosNameController : ControllerBase
    {
        private readonly string connectionString = "server=localhost;database=bicicleta;user=root;password=''";

        // O modelo de dados pode ser o mesmo, mas vamos manter a consistência
        // com o que o frontend espera (Name, Score, Posicao com letra maiúscula)
        public class PlayerRank
        {
            public string? Name { get; set; }
            public int Score { get; set; }
            public long Posicao { get; set; } // Usar long para ROW_NUMBER
        }

        // A rota agora espera um parâmetro 'name'. Ex: GET /api/GetPosName/Joao
        [HttpGet("{name}")]
        public IActionResult GetPlayerRanking(string name)
        {
            try
            {
                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                // Esta subquery é um pouco ineficiente. Podemos otimizá-la.
                // Vamos criar o ranking de todos e depois encontrar o jogador específico.
                string query = @"
                    SELECT name, score, posicao
                    FROM (
                        SELECT name, score, ROW_NUMBER() OVER (ORDER BY score DESC) AS posicao
                        FROM scores
                    ) AS ranking_completo
                    WHERE name = @name;
                ";

                using var cmd = new MySqlCommand(query, connection);
                // Usa o 'name' que veio da URL, não a variável estática!
                cmd.Parameters.AddWithValue("@name", name);

                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var playerRank = new PlayerRank
                    {
                        Name = reader.GetString("name"),
                        Score = reader.GetInt32("score"),
                        Posicao = reader.GetInt64("posicao")
                    };
                    return Ok(playerRank); // Retorna o objeto do jogador encontrado
                }
                else
                {
                    // Se o reader não encontrou nenhuma linha, o jogador não existe no banco.
                    return NotFound(new { message = "Jogador não encontrado." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao buscar ranking do jogador.", details = ex.Message });
            }
        }
    }
}
