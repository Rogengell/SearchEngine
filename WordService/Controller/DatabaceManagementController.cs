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
        _dataBase.DeleteDatabase();
    }

    [HttpPost("CreateDatabase")]
    public void CreateDatabase()
    {
        _dataBase.CreateDatabase();
    }
}