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

        public class AddModel
        {
            public string? Name { get; set; }
        }


        [HttpPost]
        public IActionResult Post([FromBody] AddModel Data)
        {
            //Values.Name = Data.Name;
            try
            { 
                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                string query = "INSERT INTO scores (name, score) VALUES (@name, @score)";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", Data.Name);
                cmd.Parameters.AddWithValue("@score", Values.Score);
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
