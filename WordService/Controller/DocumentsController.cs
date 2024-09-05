using Microsoft.AspNetCore.Mvc;
using WordService;

[ApiController]
[Route("[controller]")]
public class DocumentsController : Controller
{

    private static Database _dataBase;

    public DocumentsController(WordService.Database dataBase)
    {
        _dataBase = dataBase;
    }
    
    [HttpPost("InsertDocument")]
    public async void InsertDocument(int id, string url)
    {
        Console.WriteLine(Environment.MachineName);
        _dataBase.InsertDocument(id, url);
    }

    [HttpPost("GetDocuments")]
    public async Task<Dictionary<int, int>> GetDocuments(List<int> wordIds)
    {
        Console.WriteLine(Environment.MachineName);
        return await _dataBase.GetDocuments(wordIds);
    }

    [HttpPost("GetDocDetails")]
    public async Task<List<string>> GetDocDetails(List<int> docIds)
    {
        Console.WriteLine(Environment.MachineName);
        return await _dataBase.GetDocDetails(docIds);
    }
}