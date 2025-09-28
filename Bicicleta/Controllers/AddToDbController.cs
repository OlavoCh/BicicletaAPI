using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bicicleta.Data;
using MySql.Data.MySqlClient;
using Mysqlx;

namespace Bicicleta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddToDbController : ControllerBase
    {
        private readonly string connectionString = "server=localhost;database=bicicleta;user=root;password=''";

        [HttpPost]
        public IActionResult Post([FromBody] string name)
        {
            try
            {
                int score = Values.score;

                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                string query = "INSERT INTO scores (name, score) VALUES (@name, @score)";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@score", score);
                cmd.ExecuteNonQuery();

                connection.Close();
                return Ok(new { message = "Score inserido com sucesso!" });

            }
            catch (MySqlException ex)
            {
                if(ex.Number == 1062)
                {
                    return BadRequest(new { error = "Nome já existe!" });
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
