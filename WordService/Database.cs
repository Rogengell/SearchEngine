using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Data.SqlClient;

namespace WordService
{
    public class Database
    {   
        private Coordinator _coordinator = new Coordinator();


        public Database()
        {
        }

        internal async Task InsertAllWords(Dictionary<string, int> res)
        {
            foreach(var p in res)
            {
                var connection = await _coordinator.GetWordConnection(p.Key);
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = @"INSERT INTO Words(id, name) VALUES(@id,@name)";

                    var paramName = command.CreateParameter();
                    paramName.ParameterName = "name";
                    paramName.Value = p.Key;
                    command.Parameters.Add(paramName);

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "id";
                    paramId.Value = p.Value;
                    command.Parameters.Add(paramId);
                    
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                connection.Close();
            }
        }

        internal async Task<Dictionary<string, int>> GetAllWords()
        {
            Dictionary<string, int> res = new Dictionary<string, int>();

            await foreach(var connections in _coordinator.GetAllWordConnections())
            {
                var selectCmd = connections.CreateCommand();
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

                connections.Close();
            }
            return res;
        }
        internal async Task InsertAllOcc(int docId, ISet<int> wordIds)
        {
            var connection = await _coordinator.GetOccurrenceConnection();
            using (var transaction = connection.BeginTransaction())
            {

                var command = connection.CreateCommand();

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
            connection.Close();
        }
        internal async Task InsertDocument(int id, string url)
        {
            var connection = await _coordinator.GetDocumentConnection();
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = "INSERT INTO Documents(id, url) VALUES(@id,@url)";

            var pName = new SqlParameter("url", url);
            insertCmd.Parameters.Add(pName);

            var pCount = new SqlParameter("id", id);
            insertCmd.Parameters.Add(pCount);

            insertCmd.ExecuteNonQuery();
            connection.Close();
        }


        internal string AsString(List<int> x)
        {
            return string.Concat("(", string.Join(',', x.Select(i => i.ToString())), ")");
        }

        internal async Task<Dictionary<int, int>> GetDocuments(List<int> wordIds)
        {
            var res = new Dictionary<int, int>();

            var sql = @"SELECT docId, COUNT(wordId) AS count FROM Occurrences WHERE wordId IN " + AsString(wordIds) + " GROUP BY docId ORDER BY count DESC";

            var connection = await _coordinator.GetOccurrenceConnection();
            var selectCmd = connection.CreateCommand();
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
            connection.Close();
            return res;
        }

        internal async Task<List<string>> GetDocDetails(List<int> docIds)
        {
            List<string> res = new List<string>();

            var connection = await _coordinator.GetDocumentConnection();
            var selectCmd = connection.CreateCommand();
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
            connection.Close();
            return res;
        }


        internal void Execute(IDbConnection dbConnection,string sql)
        {   
            using var trans = dbConnection.BeginTransaction();
            var cmd = dbConnection.CreateCommand();
            cmd.Transaction = trans;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            trans.Commit();
        }

        internal async Task ReCreateDatabase()
        {
            await foreach(var connection in _coordinator.GetAllConnections())
            { 
                Execute(connection, "DROP TABLE IF EXISTS Occurrences");
                Execute(connection, "DROP TABLE IF EXISTS Words");
                Execute(connection, "DROP TABLE IF EXISTS Documents");
                connection.Close();
            }
            await CreateDatabase();
        }

        public async Task CreateDatabase()
        {   
            var connectionDoc = await _coordinator.GetDocumentConnection(); 
            Execute(connectionDoc, "CREATE TABLE Documents(id INTEGER PRIMARY KEY, url VARCHAR(500))");
            connectionDoc.Close();
            var connectionOCc = await _coordinator.GetOccurrenceConnection();
            Execute(connectionOCc, "CREATE TABLE Occurrences(wordId INTEGER, docId INTEGER)");
            connectionOCc.Close();
            await foreach (var connection in _coordinator.GetAllWordConnections())
            {
                Execute(connection, "CREATE TABLE Words(id INTEGER PRIMARY KEY, name VARCHAR(500))");
                connection.Close();
            }
            
        }
    }
}
