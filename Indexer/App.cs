using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using RestSharp;

namespace Indexer
{
    public class App
    {
        public void Run()
        {
            //HttpClient api = new() { BaseAddress = new Uri("http://WordServices.localhost:80/")};
            
            RestClient restClient = new RestClient("http://word-service");

            var urlDelete = "DatabaceManagement/DeleteDatabase";
            restClient.DeleteAsync(new RestRequest(urlDelete));

            Thread.Sleep(2000);
            
            var urlCreate = "DatabaceManagement/CreateDatabase";
            restClient.PostAsync(new RestRequest(urlCreate));

            Thread.Sleep(2000);
            Console.WriteLine("DONE!");
            /*
            var urlDelete = "DatabaceManagement/DeleteDatabase";
            var temp = new HttpRequestMessage(HttpMethod.Delete, urlDelete);
            Console.WriteLine(temp.RequestUri);
            var responseDelete = api.Send(temp);
            if(responseDelete.StatusCode.Equals(HttpStatusCode.OK))
            {
                Console.WriteLine("OK delete");
            }
            else
            {
                Console.WriteLine("not ok");
            }

            var urlCreate = "DatabaceManagement/CreateDatabase";
            var responseCreate = api.Send(new HttpRequestMessage(HttpMethod.Post, urlCreate));
            if(responseCreate.StatusCode.Equals(HttpStatusCode.OK))
            {
                Console.WriteLine("OK Create");
            }
            else
            {
                Console.WriteLine("not ok");
            }
            */
            Crawler crawler = new Crawler();

            var directoryArray = new DirectoryInfo("maildir").GetDirectories();
            var directories = new List<DirectoryInfo>(directoryArray).OrderBy(d => d.Name).AsEnumerable();
            
            DateTime start = DateTime.Now;
            foreach (var directory in directories)
            {
                crawler.IndexFilesIn(directory, new List<string> { ".txt"});
            }
            
            TimeSpan used = DateTime.Now - start;
            Console.WriteLine("DONE! used " + used.TotalMilliseconds); 
        }
    }
}
