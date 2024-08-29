using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using System.Text;
using System.Net.Http.Json;
using System.Text.Json.Serialization;


namespace ConsoleSearch
{
    public class SearchLogic
    {
        Dictionary<string, int> mWords;
        private new HttpClient api = new() { BaseAddress = new Uri("http://localhost:5120")};

        public SearchLogic()
        {
            var urlAlleWord = "Word/GetAllWords";
            HttpResponseMessage responseAlleWord = api.Send(new HttpRequestMessage(HttpMethod.Get, urlAlleWord));
            var responce = responseAlleWord.Content.ReadAsStringAsync().Result;
            mWords = JsonSerializer.Deserialize<Dictionary<string, int>>(responce);
        }

        public int GetIdOf(string word)
        {
            if (mWords.ContainsKey(word))
                return mWords[word];
            return -1;
        }

        public Dictionary<int, int> GetDocuments(List<int> wordIds)
        { 
            if(wordIds.Count>0){
                var getDocuments = new HttpRequestMessage(HttpMethod.Post, "Documents/GetDocuments")
                {
                    Content = new StringContent(JsonSerializer.Serialize(wordIds), Encoding.UTF8, "application/json")
                };
                HttpResponseMessage responce = api.Send(getDocuments);
                var body = responce.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrWhiteSpace(body))
                {
                    return JsonSerializer.Deserialize<Dictionary<int, int>>(body);
                }
            }
            return new Dictionary<int, int>();
        }

        public List<string> GetDocumentDetails(List<int> docIds)
        {
            if(docIds.Count>0){
                var getDocumentDetails = new HttpRequestMessage(HttpMethod.Post, "Documents/GetDocDetails")
                {
                    Content = new StringContent(JsonSerializer.Serialize(docIds), Encoding.UTF8, "application/json")
                };
                HttpResponseMessage responce = api.Send(getDocumentDetails);
                string body = responce.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrWhiteSpace(body))
                {
                    return JsonSerializer.Deserialize<List<string>>(body); 
                }
            }
            return new List<string>();
        }
    }
}