using Microsoft.AspNetCore.Mvc;
using WordService;

[ApiController]
[Route("[controller]")]
public class WordController : Controller
{
    private static Database _dataBase;

    public WordController(WordService.Database database)
    {
        _dataBase = database;
    }

    [HttpPost("InsertAllWords")]
    public async void InsertAllWords(Dictionary<string, int> res)
    {
        try
        {

        }
        catch (System.Exception ex)
        {
            Console.WriteLine("something went wrong in InsertAllWords " + ex.Message);
            throw;
        }
        Console.WriteLine(Environment.MachineName);
        await _dataBase.InsertAllWords(res);
    }

    [HttpGet("GetAllWords")]
    public async Task<Dictionary<string, int>> GetAllWords()
    {
        try
        {

        }
        catch (System.Exception ex)
        {
            Console.WriteLine("something went wrong in GetAllWords " + ex.Message);
            throw;
        }
        Console.WriteLine(Environment.MachineName);
        return await _dataBase.GetAllWords();
    }
}