﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Indexer
{
    public class Crawler
    {
        private readonly char[] sep = " \\\n\t\"$'!,?;.:-_**+=)([]{}<>/@&%€#".ToCharArray();

        private new HttpClient api = new() { BaseAddress = new Uri("http://word-service")};
        private Dictionary<string, int> words = new Dictionary<string, int>();
        private Dictionary<string, int> documents = new Dictionary<string, int>();

        //Return a dictionary containing all words (as the key)in the file
        // [f] and the value is the number of occurrences of the key in file.
        private ISet<string> ExtractWordsInFile(FileInfo f)
        {
            ISet<string> res = new HashSet<string>();
            var content = File.ReadAllLines(f.FullName);
            foreach (var line in content)
            {
                foreach (var aWord in line.Split(sep, StringSplitOptions.RemoveEmptyEntries))
                {
                    res.Add(aWord);
                }
            }

            return res;
        }

        private ISet<int> GetWordIdFromWords(ISet<string> src)
        {
            ISet<int> res = new HashSet<int>();

            foreach ( var p in src)
            {
                res.Add(words[p]);
            }
            return res;
        }

        // Return a dictionary of all the words (the key) in the files contained
        // in the directory [dir]. Only files with an extension in
        // [extensions] is read. The value part of the return value is
        // the number of occurrences of the key.
        public void IndexFilesIn(DirectoryInfo dir, List<string> extensions)
        {
            Console.WriteLine("Crawling " + dir.FullName);

            foreach (var file in dir.EnumerateFiles())
            {
                if (extensions.Contains(file.Extension))
                {
                    documents.Add(file.FullName, documents.Count + 1);

                    var documentMessage = new HttpRequestMessage(HttpMethod.Post, "Documents/InsertDocument?id=" + documents[file.FullName]  + "&url=" + Uri.EscapeDataString(file.FullName));
                    api.Send(documentMessage);

                    Dictionary<string, int> newWords = new Dictionary<string, int>();
                    ISet<string> wordsInFile = ExtractWordsInFile(file);
                    foreach (var aWord in wordsInFile)
                    {
                        if (!words.ContainsKey(aWord))
                        {
                            words.Add(aWord, words.Count + 1);
                            newWords.Add(aWord, words[aWord]);
                        }
                    }
                    var newWordtMessage = new HttpRequestMessage(HttpMethod.Post, "Word/InsertAllWords")
                    {
                        Content = new StringContent(JsonSerializer.Serialize(newWords), Encoding.UTF8, "application/json")
                    };
                    api.Send(newWordtMessage);

                    var insertAllOccMessage = new HttpRequestMessage(HttpMethod.Post, "Occurrences/InsertAllOcc?docId=" + documents[file.FullName])
                    {
                        Content = new StringContent(JsonSerializer.Serialize(GetWordIdFromWords(wordsInFile)), Encoding.UTF8, "application/json")
                    };
                    api.Send(insertAllOccMessage);
                }
            }

            foreach (var d in dir.EnumerateDirectories())
            {
                IndexFilesIn(d, extensions);
            }
        }
    }
}
