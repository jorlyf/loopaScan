using System;
using System.Net;
using System.IO;

using loopaScan.Infrastructure;

namespace loopaScan.Models
{
    class Scan
    {
        public string IP;
        public string Port;
        public bool IsSuccess;
        public string Content;
        public Scan(string ip, string port)
        {
            IP = ip;
            Port = port;
            Run();
        }
        private void Run()
        {
            IsSuccess = false;
            try
            {
                WebRequest request = (HttpWebRequest)WebRequest.Create($"http://{IP}:{Port}");
                request.Timeout = 10_000; // ms
                using (WebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                    {
                        Content = stream.ReadLine();
                    }
                    IsSuccess = true;
                    response.Close();
                }
            }
            catch { }
        }
        public void Save(string sessionName)
        {
            string path = $"{Directories.IPfiles}\\{sessionName}_SCANNED.txt";
            File.AppendAllText(path, $"{IP} --- {Content}\n");
        }
    }
}
