using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

[ApiController]
[Route("[controller]")]
public class WordController : Controller
{
    private SqlConnection _connection;

    internal void InsertAllWords(Dictionary<string, int> res)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            var command = _connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = @"INSERT INTO Words(id, name) VALUES(@id,@name)";

            var paramName = command.CreateParameter();
            paramName.ParameterName = "name";
            command.Parameters.Add(paramName);

            var paramId = command.CreateParameter();
            paramId.ParameterName = "id";
            command.Parameters.Add(paramId);

            // Insert all entries in the res
            
            foreach (var p in res)
            {
                paramName.Value = p.Key;
                paramId.Value = p.Value;
                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }
    }

    public Dictionary<string, int> GetAllWords()
    {
        Dictionary<string, int> res = new Dictionary<string, int>();
    
        var selectCmd = _connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Words";

        using (var reader = selectCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var w = reader.GetString(1);
                
                res.Add(w, id);
            }
        }
        return res;
    }
}