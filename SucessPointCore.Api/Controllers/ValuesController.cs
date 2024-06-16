using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SucessPointCore.Helpers;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SucessPointCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection();
                conn.ConnectionString = AppConfigHelper.ConnectionString;
                conn.Open();
                return new string[] { "Connection Opened Sucessfully", "value2" };
            }
            catch (Exception ex)
            {

                return new string[] { "Connection Opened Failed", ex.Message };
            }
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
