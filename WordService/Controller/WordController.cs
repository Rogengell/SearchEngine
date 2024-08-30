using Microsoft.AspNetCore.Mvc;
using WordService;

[ApiController]
[Route("[controller]")]
public class WordController : Controller
{
    private Database _dataBase = Database.getInstance();

    [HttpPost("InsertAllWords")]
    public void InsertAllWords(Dictionary<string, int> res)
    {
        Console.WriteLine(Environment.MachineName);
        _dataBase.InsertAllWords(res);
    }

    [HttpGet("GetAllWords")]
    public Dictionary<string, int> GetAllWords()
    {
        Console.WriteLine(Environment.MachineName);
        return _dataBase.GetAllWords();
    }
}