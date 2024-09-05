using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using System.Text;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Diagnostics;
using RestSharp;

using System.Threading.Tasks;

namespace ConsoleSearch
{
    public class SearchLogic
    {
        Dictionary<string, int> mWords;
        //private new HttpClient api = new() { BaseAddress = new Uri("http://WordServices.localhost:80") };
        private static RestClient restClient = new RestClient("http://word-service");

        public SearchLogic()
        {
            int Count = 0;
            while (mWords == null){
                mWords = GetWords();
                Count++;
                if (Count > 3)
                {
                    System.Environment.Exit(0);
                }
            }
        }

        private Dictionary<string, int> GetWords()
        {
            var urlAlleWord = "Word/GetAllWords";
            var task = restClient.GetAsync<Dictionary<string, int>>(new RestRequest(urlAlleWord));
            //var request = new RestRequest(urlAlleWord);
            //var response = restClient.ExecuteGetAsync(request);
            //var data = JsonSerializer.Deserialize<Dictionary<string, int>>(response.Result.Content);
            Thread.Sleep(1000);

            if(task?.Status == TaskStatus.RanToCompletion)
            {
                Console.WriteLine("Task");
                return task.Result;
            }
            Console.WriteLine("Error");
            return null;
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

                var urlAlleWord = "Documents/GetDocuments";
                Console.WriteLine("get Doc");

                var request = new RestRequest(urlAlleWord,Method.Post);

                request.AddJsonBody(JsonSerializer.Serialize(wordIds));

                var task = restClient.PostAsync<Dictionary<int, int>>(request);
                /*
                var getDocuments = new HttpRequestMessage(HttpMethod.Post, "Documents/GetDocuments")
                {
                    Content = new StringContent(JsonSerializer.Serialize(wordIds), Encoding.UTF8, "application/json")
                };
                HttpResponseMessage responce = api.Send(getDocuments);
                var body = responce.Content.ReadAsStringAsync().Result;
                */

                Thread.Sleep(1000);

                if (task.Result != null || task.Result.Count > 0)
                {
                    return task.Result;
                }

            }
            return new Dictionary<int, int>();
        }

        public List<string> GetDocumentDetails(List<int> docIds)
        {
            if(docIds.Count>0){

                var urlAlleWord = "Documents/GetDocDetails";
                Console.WriteLine("get Details");
                var task = restClient.GetAsync<Dictionary<string, int>>(new RestRequest(urlAlleWord).AddParameter("application/json", JsonSerializer.Serialize(docIds), ParameterType.RequestBody));
                /*
                var getDocumentDetails = new HttpRequestMessage(HttpMethod.Post, "Documents/GetDocDetails")
                {
                    Content = new StringContent(JsonSerializer.Serialize(docIds), Encoding.UTF8, "application/json")
                };
                HttpResponseMessage responce = api.Send(getDocumentDetails);
                string body = responce.Content.ReadAsStringAsync().Result;
                */
                
                Thread.Sleep(1000);
                
                if (!string.IsNullOrWhiteSpace(task.Result.ToString()))
                {
                    return JsonSerializer.Deserialize<List<string>>(task.Result.ToString()); 
                }
            }
            return new List<string>();
        }
    }
}