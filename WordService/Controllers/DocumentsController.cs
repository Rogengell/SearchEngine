using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


[ApiController]
[Route("[controller]")]
public class DocumentsController : Controller
{

    private SqlConnection _connection;

    public void InsertDocument(int id, string url)
    {
        var insertCmd = _connection.CreateCommand();
        insertCmd.CommandText = "INSERT INTO Documents(id, url) VALUES(@id,@url)";

        var pName = new SqlParameter("url", url);
        insertCmd.Parameters.Add(pName);

        var pCount = new SqlParameter("id", id);
        insertCmd.Parameters.Add(pCount);

        insertCmd.ExecuteNonQuery();
    }


    private string AsString(List<int> x)
    {
        return string.Concat("(", string.Join(',', x.Select(i => i.ToString())), ")");
    }

    public Dictionary<int, int> GetDocuments(List<int> wordIds)
    {
        var res = new Dictionary<int, int>();

        var sql = @"SELECT docId, COUNT(wordId) AS count FROM Occurrences WHERE wordId IN " + AsString(wordIds) + " GROUP BY docId ORDER BY count DESC;";

        var selectCmd = _connection.CreateCommand();
        selectCmd.CommandText = sql;

        using (var reader = selectCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var docId = reader.GetInt32(0);
                var count = reader.GetInt32(1);
                
                res.Add(docId, count);
            }
        }

        return res;
    }

    public List<string> GetDocDetails(List<int> docIds)
    {
        List<string> res = new List<string>();

        var selectCmd = _connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Documents WHERE id IN " + AsString(docIds);

        using (var reader = selectCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var url = reader.GetString(1);

                res.Add(url);
            }
        }
        return res;
    }
}