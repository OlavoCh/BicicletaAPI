using Bicicleta.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Xml.Linq;


namespace Bicicleta.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]

    public class UpdScore : ControllerBase

    {
        private readonly string connectionString = "Server=localhost;Database=bicicleta;User=root;Password=12345678;";

        public class UpdModel { 
            public string? Name { get; set; }
            public int score { get; set; }
        }


        [HttpPut]
        
        public IActionResult Post([FromBody] UpdModel Data)
        {
            try
            {

                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                string query = "UPDATE scores SET score = @score WHERE name = @name";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@score", Data.score);
                cmd.Parameters.AddWithValue("@name", Data.Name);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound(new { message = "Usuário não encontrado." });
                }
                Values.Score = 0;
                Values.TextString = null;
                return Ok(new { message = "Score atualizado com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao atualizar o score.", error = ex.Message });
            }

            
            
        }
    }
}
