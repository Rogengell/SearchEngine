using System;
using System.Formats.Tar;
using System.IO;
using System.IO.Compression;

namespace Indexer
{
    class Program
    {
        static void Main(string[] args)
        {
            DecompressGzipFile("enron/mikro.tar.gz", "mails.tar");
            if (Directory.Exists("maildir")) Directory.Delete("maildir", true);

            string binPath = AppDomain.CurrentDomain.BaseDirectory;

            if (binPath.EndsWith("bin\\Debug\\net7.0\\") || binPath.EndsWith("bin\\Release\\net7.0\\"))
            {
                if (File.Exists(binPath + "._maildir")) File.Delete(binPath + "._maildir");
            }
            TarFile.ExtractToDirectory("mails.tar", ".", false);

            if (true)
            {
                new Renamer().Crawl(new DirectoryInfo("maildir"));
                new App().Run();
            }
        }

        static void DecompressGzipFile(string compressedFilePath, string decompressedFilePath)
        {
            using (FileStream compressedFileStream = File.OpenRead(compressedFilePath))
            {
                using (FileStream decompressedFileStream = File.Create(decompressedFilePath))
                {
                    using (GZipStream gzipStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
                    {
                        gzipStream.CopyTo(decompressedFileStream);
                    }
                }
            }
        }
    }
}