using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace PrisonGame
{
    class Program
    {
        private static readonly string DESKTOP_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private const string FOLDER = "swf sources\\prison game\\swf links";

        private static readonly string[] FILE_NAMES = new string[]
        {
            "copy all as powershell.txt",
            "copy all as har.txt",
        };

        private const string START = "http";
        private const string SWF = ".swf";

        private static List<string> Lines = null;
        //private static int totalDownloaded = 0;
        //private static DateTime dt;

        static void Main(string[] args)
        {
            Convert();
            //Download1("dlt");
            Console.ReadKey();
        }

        private static void Convert()
        {
            string path = Path.Combine(DESKTOP_PATH, FOLDER, FILE_NAMES[1]);
            Console.WriteLine(path);

            Lines = File.ReadAllLines(path).ToList();
            Console.WriteLine($"Всего строк в файле: {Lines.Count}");

            Lines = Lines.Where(line => line.Contains(SWF)).OrderBy(line => line).ToList();
            Console.WriteLine($"Нужных строк: {Lines.Count}");
            Lines = Lines.Distinct().ToList();
            Console.WriteLine($"Без повторов: {Lines.Count}");

            //RemoveStart();
            //File.WriteAllLines(DESKTOP_PATH + "/lines.txt", Lines);
        }

        private static void RemoveStart()
        {
            for (int i = 0, length = Lines.Count; i < length; i++)
            {
                string line = Lines[i];

                int startIndex = line.IndexOf(START);
                startIndex = Math.Max(0, startIndex);
                line = line.Substring(startIndex);

                line = line.TrimEnd('"', ',');

                Lines[i] = line;
                Console.WriteLine($"{i}: {Lines[i]}");
            }
        }

        private static void Download(string folderName)
        {
            string path = Path.Combine(DESKTOP_PATH, "lines.txt");
            Lines = File.ReadAllLines(path).ToList();

            for (int i = 0, length = Lines.Count; i < length; i++)
            {
                string line = Lines[i];
                string end = "net/";
                int index = line.IndexOf(end);
                string name = line.Substring(index + end.Length);
                string savePath = Path.Combine(DESKTOP_PATH, folderName, name);

                //Console.WriteLine($"{i}: {name}");
                //Console.WriteLine($"{i}: {savePath}");
                //Console.WriteLine($"{i}: {Lines[i]}");
                //Console.WriteLine($"{i}: {line}");

                using (WebClient web = new WebClient())
                {
                    //web.DownloadProgressChanged += wc_DownloadProgressChanged;
                    //web.DownloadDataCompleted += Web_DownloadDataCompleted;
                    Uri uri = new Uri(line);
                    web.DownloadFileAsync(uri, savePath);
                }
            }
        }

        private static void Download1(string folderName)
        {
            string path = Path.Combine(DESKTOP_PATH, "lines.txt");
            Lines = File.ReadAllLines(path).ToList();

            using (WebClient web = new WebClient())
            {
                //web.DownloadDataCompleted += Web_DownloadDataCompleted;

                for (int i = 0, length = Lines.Count; i < length; i++)
                {
                    string line = Lines[i];
                    string end = "net/";
                    int index = line.IndexOf(end);
                    string name = line.Substring(index + end.Length);
                    string savePath = Path.Combine(DESKTOP_PATH, folderName, name);

                    //Console.WriteLine($"{i}: {name}");
                    //Console.WriteLine($"{i}: {savePath}");
                    //Console.WriteLine($"{i}: {Lines[i]}");
                    //Console.WriteLine($"{i}: {line}");
                
                    //web.DownloadProgressChanged += wc_DownloadProgressChanged;

                    while(web.IsBusy)
                    {
                        Thread.Sleep(100);
                    }

                    Uri uri = new Uri(line);
                    web.DownloadFileAsync(uri, savePath);
                }
            }
        }

        //private static void Web_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        //{
        //    dt = DateTime.Now;
        //    Console.WriteLine(dt.ToLongTimeString() + " / " + (++totalDownloaded));
        //}
    }
}
