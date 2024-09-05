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
        try
        {
            Console.WriteLine(Environment.MachineName);
            await _dataBase.InsertDocument(id, url);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("something went wrong in InsertDocument " + ex.Message);
            throw;
        }
    }

    [HttpPost("GetDocuments")]
    public async Task<Dictionary<int, int>> GetDocuments(List<int> wordIds)
    {
        try
        {
            Console.WriteLine(Environment.MachineName);
            return await _dataBase.GetDocuments(wordIds);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("something went wrong in GetDocuments " + ex.Message);
            throw;
        }
    }

    [HttpPost("GetDocDetails")]
    public async Task<List<string>> GetDocDetails(List<int> docIds)
    {
        try
        {
            Console.WriteLine(Environment.MachineName);
            return await _dataBase.GetDocDetails(docIds);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("something went wrong in GetDocDetails " + ex.Message);
            throw;
        }
    }
}