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
        try
        {
            Console.WriteLine(Environment.MachineName);
            await _dataBase.ReCreateDatabase();
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Something went wrong recreating the database" + ex.Message);
            throw;
        }
    }
}