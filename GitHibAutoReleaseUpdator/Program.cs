using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHibAutoReleaseUpdator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GitHibAutoReleaseUpdator.GitHubUpdator.CheckUpdates(args, "Mrgaton", "YoutuveDownloader");

            Console.ReadLine();

            ZipFile.CreateFromDirectory("C:\\Users\\Mrgaton\\Mega\\Escritorio", Path.Combine(Path.GetTempPath(), "aaa.zip"),CompressionLevel.Optimal,false,Encoding.UTF8);
        }
    }
}
