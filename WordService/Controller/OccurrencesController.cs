using Microsoft.AspNetCore.Mvc;
using WordService;

[ApiController]
[Route("[controller]")]
public class Occurrences : Controller
{
    private static Database _dataBase;

    public Occurrences(WordService.Database dataBase)
    {
        _dataBase = dataBase;
    }
    [HttpPost("InsertAllOcc")]
    public async void InsertAllOcc(int docId, ISet<int> wordIds)
    {
        Console.WriteLine(Environment.MachineName);
        await _dataBase.InsertAllOcc(docId, wordIds);
    }
}