using Microsoft.AspNetCore.Mvc;
using WordService;

[ApiController]
[Route("[controller]")]
public class DatabaceManagementController : Controller
{

    private static Database _dataBase;

    public DatabaceManagementController(WordService.Database database)
    {
        _dataBase = database;
    }

    [HttpDelete("reCreateDatabase")]
    public async void ReCreateDatabase()
    {
        Console.WriteLine(Environment.MachineName);
        await _dataBase.ReCreateDatabase();
    }
}