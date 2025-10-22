using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bicicleta.Data;
using MySql.Data.MySqlClient;
using Mysqlx;
using System.Diagnostics.Contracts;

namespace Bicicleta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Renomeie o controller para AddUser para corresponder à URL no seu JS
    public class AddToDbController : ControllerBase
    {
        private readonly string connectionString = "Server=localhost;Database=bicicleta;User=root;Password=12345678;";

        public class DataMode{
            public string? Name { get; set; }
            public int Score { get; set; }
        }

        [HttpPost]
        public IActionResult Post([FromBody] DataMode data)
        {
            try
            {
                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                string query = "INSERT INTO scores (name, score) VALUES (@name, @score)";
                using var cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@name", data.Name);
                // Use o score recebido no corpo da requisição
                cmd.Parameters.AddWithValue("@score", data.Score);
                cmd.ExecuteNonQuery();

                // Não precisa fechar a conexão aqui, o 'using' já faz isso.
                return Ok(new { message = "Score inserido com sucesso!" });
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062) // 1062 é o código para entrada duplicada (UNIQUE key)
                {
                    return BadRequest(new { error = "Este nome já foi registrado!" });
                }
                return StatusCode(500, new { error = "Erro ao inserir score no banco de dados.", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro inesperado.", details = ex.Message });
            }
        }
    }
}