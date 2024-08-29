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
        _dataBase.InsertDocument(id,url);
    }

    [HttpGet("GetDocuments")]
    public Dictionary<int, int> GetDocuments(List<int> wordIds)
    {
        return _dataBase.GetDocuments(wordIds);
    }
    
    [HttpGet("GetDocDetails")]
    public List<string> GetDocDetails(List<int> docIds)
    {
        return _dataBase.GetDocDetails(docIds);
    }
}