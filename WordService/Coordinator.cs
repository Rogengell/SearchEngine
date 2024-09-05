using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;

public class Coordinator : IDisposable
{
    private IDictionary<string, SqlConnection> ConnectionCache = new Dictionary<string, SqlConnection>();
    private const string DOCUMENT_DB = "document-db";
    private const string OCCURRENCE_DB = "occurrence-db";
    private const string SHORT_WORD_DB = "short-word-db";
    private const string MEDIUM_WORD_DB = "medium-word-db";
    private const string LONG_WORD_DB = "long-word-db";

    public Coordinator()
    {/*
        var DOCUMENT_DB_connection = new SqlConnection($"Server={DOCUMENT_DB};User Id=sa;Password=SuperSecret7!;Encrypt=false;");
        ConnectionCache.Add(DOCUMENT_DB,DOCUMENT_DB_connection);
        var OCCURRENCE_DB_connection = new SqlConnection($"Server={OCCURRENCE_DB};User Id=sa;Password=SuperSecret7!;Encrypt=false;");
        ConnectionCache.Add(OCCURRENCE_DB,OCCURRENCE_DB_connection);
        var SHORT_WORD_DB_connection = new SqlConnection($"Server={SHORT_WORD_DB};User Id=sa;Password=SuperSecret7!;Encrypt=false;");
        ConnectionCache.Add(SHORT_WORD_DB,SHORT_WORD_DB_connection);
        var MEDIUM_WORD_DB_connection = new SqlConnection($"Server={MEDIUM_WORD_DB};User Id=sa;Password=SuperSecret7!;Encrypt=false;");
        ConnectionCache.Add(MEDIUM_WORD_DB,MEDIUM_WORD_DB_connection);
        var LONG_WORD_DB_connection = new SqlConnection($"Server={LONG_WORD_DB};User Id=sa;Password=SuperSecret7!;Encrypt=false;");
        ConnectionCache.Add(LONG_WORD_DB,LONG_WORD_DB_connection);*/
    }

    public async Task<SqlConnection> GetDocumentConnection()
    {
        return await GetConnectionByServerName(DOCUMENT_DB);
    }

    public async Task<SqlConnection> GetOccurrenceConnection()
    {
        return await GetConnectionByServerName(OCCURRENCE_DB);
    }
    
    public async Task<SqlConnection> GetWordConnection(string word)
    {
        int count = word.Length;
        switch(count)
        {
            case var l when (count <= 10):
                return await GetConnectionByServerName(SHORT_WORD_DB);
            case var l when (count > 10 && count <= 20):
                return await GetConnectionByServerName(MEDIUM_WORD_DB);
            case var l when (count >= 21):
                return await GetConnectionByServerName(LONG_WORD_DB);
            default:
                throw new InvalidDataException();
        }
    }

    public async IAsyncEnumerable<SqlConnection> GetAllConnections()
    {
        yield return await GetDocumentConnection();
        yield return await GetOccurrenceConnection();
        await foreach (var wordConnection in GetAllWordConnections())
        {
            yield return wordConnection;
        }
    }
    
    public async IAsyncEnumerable<SqlConnection> GetAllWordConnections()
    {
        yield return await GetConnectionByServerName(SHORT_WORD_DB);
        yield return await GetConnectionByServerName(MEDIUM_WORD_DB);
        yield return await GetConnectionByServerName(LONG_WORD_DB);
    }

    private async Task<SqlConnection> GetConnectionByServerName(string serverName)
    {
        /*
        if (ConnectionCache.TryGetValue(serverName, out var connection))
        {
            if (connection.State == ConnectionState.Open)
            {
                return connection;
            }
        }*/
        
        var connection = new SqlConnection($"Server={serverName};User Id=sa;Password=SuperSecret7!;Encrypt=false;");
        await connection.OpenAsync();
        return connection;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}