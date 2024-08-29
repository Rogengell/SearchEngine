using Microsoft.AspNetCore.Mvc;
using WordService;

[ApiController]
[Route("[controller]")]
public class WordController : Controller
{
    private Database _dataBase = new Database();

    [HttpPost("InsertAllWords")]
    public void InsertAllWords(Dictionary<string, int> res)
    {
        _dataBase.InsertAllWords(res);
    }

    [HttpGet("GetAllWords")]
    public Dictionary<string, int> GetAllWords()
    {
        return _dataBase.GetAllWords();
    }
}