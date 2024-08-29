using Microsoft.AspNetCore.Mvc;
using WordService;

[ApiController]
[Route("[controller]")]
public class Occurrences : Controller
{
    private Database _dataBase = Database.getInstance();

    [HttpPost("InsertAllOcc")]
    public void InsertAllOcc(int docId, ISet<int> wordIds)
    {
       _dataBase.InsertAllOcc(docId,wordIds);
    }
}