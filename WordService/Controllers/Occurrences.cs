using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


[ApiController]
[Route("[controller]")]
public class Occurrences : Controller
{
    private SqlConnection _connection;
    internal void InsertAllOcc(int docId, ISet<int> wordIds)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            var command = _connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = @"INSERT INTO Occurrences(wordId, docId) VALUES(@wordId,@docId)";

            var paramwordId = command.CreateParameter();
            paramwordId.ParameterName = "wordId";
            
            command.Parameters.Add(paramwordId);

            var paramDocId = command.CreateParameter();
            paramDocId.ParameterName = "docId";
            paramDocId.Value = docId;

            command.Parameters.Add(paramDocId);

            foreach (var p in wordIds)
            {
                paramwordId.Value = p;
                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }
    }
}