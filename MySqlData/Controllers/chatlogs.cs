using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MySqlData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatlogsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ChatlogsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetLogs")]
        public JsonResult GetLogs()
        {
            string query = "SELECT * FROM Chatlogs";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("MyDatabaseConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        [Route("InsertLogs")]
        public async Task<JsonResult> InsertLogs([FromBody] Logs log)
        {
            string query = "INSERT INTO Chatlogs (ConversationID, Username, IssueID, Message, TimeSent, IssueSolved) VALUES(@ConversationID, @Username, @IssueID, @Message, @TimeSent, @IssueSolved)";
            string sqlDatasource = _configuration.GetConnectionString("MyDatabaseConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                await myCon.OpenAsync();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // Set parameter values
                    myCommand.Parameters.AddWithValue("@ConversationID", log.ConversationID);
                    myCommand.Parameters.AddWithValue("@Username", log.Username);
                    myCommand.Parameters.AddWithValue("@IssueID", log.IssueID);
                    myCommand.Parameters.AddWithValue("@Message", log.Message);
                    myCommand.Parameters.AddWithValue("@TimeSent", log.TimeSent);
                    myCommand.Parameters.AddWithValue("@IssueSolved", log.IssueSolved);

                    await myCommand.ExecuteNonQueryAsync();
                }
            }
            return new JsonResult("Added Successfully");
        }

        // Define Logs class here
        public class Logs
        {
            public string ConversationID { get; set; }
            public string Username { get; set; }
            public int IssueID { get; set; }
            public string Message { get; set; }
            public DateTime TimeSent { get; set; }
            public int IssueSolved { get; set; }
        }
    }
}
