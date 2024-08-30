using Microsoft.AspNetCore.Mvc;
using WordService;

[ApiController]
[Route("[controller]")]
public class DocumentsController : Controller
{

    private Database _dataBase = Database.getInstance();

    [HttpPost("InsertDocument")]
    public void InsertDocument(int id, string url)
    {
        Console.WriteLine(Environment.MachineName);
        _dataBase.InsertDocument(id, url);
    }

    [HttpPost("GetDocuments")]
    public Dictionary<int, int> GetDocuments(List<int> wordIds)
    {
        Console.WriteLine(Environment.MachineName);
        return _dataBase.GetDocuments(wordIds);
    }

    [HttpPost("GetDocDetails")]
    public List<string> GetDocDetails(List<int> docIds)
    {
        Console.WriteLine(Environment.MachineName);
        return _dataBase.GetDocDetails(docIds);
    }
}