using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices.Marshalling;

namespace Bicicleta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetDataController : ControllerBase
    {
        private readonly string connectionString = "server=localhost;database=bicicleta;user=root;password=''";

        [HttpGet]
        public IActionResult Get()
        {
            var scores = new List<object>();
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT name, score FROM scores ORDER BY score DESC";
            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                scores.Add(new
                {
                    Name = reader.GetString("name"),
                    Score = reader.GetInt32("score")
                });
            }

            connection.Close();
            return Ok(scores);

        }
    }
}
