using Microsoft.AspNetCore.Mvc;
using WordService;

[ApiController]
[Route("[controller]")]
public class DatabaceManagementController : Controller
{

    private Database _dataBase = Database.getInstance();

    [HttpDelete("DeleteDatabase")]
    public void DeleteDatabase()
    {
        Console.WriteLine(Environment.MachineName);
        _dataBase.DeleteDatabase();
    }

    [HttpPost("CreateDatabase")]
    public void CreateDatabase()
    {
        Console.WriteLine(Environment.MachineName);
        _dataBase.CreateDatabase();
    }
}