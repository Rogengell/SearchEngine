using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


[ApiController]
[Route("[controller]")]
public class DatabaceManagementController : Controller
{
    [HttpGet("haha")]
    public string Get()
    {
        return "hello";
    }
    private SqlConnection _connection;

    private void Execute(string sql)
    {
        using var trans = _connection.BeginTransaction();
        var cmd = _connection.CreateCommand();
        cmd.Transaction = trans;
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
        trans.Commit();
    }

    public DatabaceManagementController()
    {
        _connection = new ("Server=localhost:;User Id=sa;Password=SuperSecret7!;Encrypt=false;");
        _connection.Open();
        
        Execute("DROP TABLE IF EXISTS Occurrences");
        Execute("DROP TABLE IF EXISTS Words");
        Execute("DROP TABLE IF EXISTS Documents");
        
        
        Execute("CREATE TABLE Documents(id INTEGER PRIMARY KEY, url VARCHAR(500))");
        Execute("CREATE TABLE Words(id INTEGER PRIMARY KEY, name VARCHAR(500))");
        Execute("CREATE TABLE Occurrences(wordId INTEGER, docId INTEGER, "
                + "FOREIGN KEY (wordId) REFERENCES Words(id), "
                + "FOREIGN KEY (docId) REFERENCES Documents(id))");
        
        //Execute("CREATE INDEX word_index ON Occ (wordId)");
        
    }
}