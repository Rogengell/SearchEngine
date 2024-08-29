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
            var getDocuments = new HttpRequestMessage(HttpMethod.Post, "Documents/GetDocumentss")
            {
                Content = new StringContent(JsonSerializer.Serialize(wordIds), Encoding.UTF8, "application/json")
            };
            HttpResponseMessage responce = api.Send(getDocuments);
            var body = responce.Content.ReadAsStringAsync().Result;
            Console.WriteLine(body + " 1");
            return JsonSerializer.Deserialize<Dictionary<int, int>>(body); 
        }

        public List<string> GetDocumentDetails(List<int> docIds)
        {
            var getDocumentDetails = new HttpRequestMessage(HttpMethod.Post, "Documents/GetDocDetails")
            {
                Content = new StringContent(JsonSerializer.Serialize(docIds), Encoding.UTF8, "application/json")
            };
            HttpResponseMessage responce = api.Send(getDocumentDetails);
            var body = responce.Content.ReadAsStringAsync().Result;
            Console.WriteLine(body + " 2");
            return JsonSerializer.Deserialize<List<string>>(body); 
        }
    }
}