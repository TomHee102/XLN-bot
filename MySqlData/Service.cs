using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class DataService
{




    private readonly IConfiguration _configuration;

    public DataService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<DataTable> GetDataFromDatabase()
    {
        var connectionString = _configuration.GetConnectionString("MyDatabaseConnection");
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT * FROM YourTable", connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    return dataTable;
                }
            }
        }
    }
}

public class YourDataModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    // Add other properties as needed
}


[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    [HttpGet]
    public IActionResult GetData()
    {
        // Fetch data from your data source (e.g., database)
        var data = new List<YourDataModel>
        {
            new YourDataModel { Id = 1, Name = "Item 1" },
            new YourDataModel { Id = 2, Name = "Item 2" }
        };
        return Ok(data);
    }
}
