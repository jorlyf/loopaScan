using System;
using System.Net;
using System.IO;

using loopaScan.Infrastructure;

namespace loopaScan.Models
{
    class Scan
    {
        public string IP;
        public bool IsSuccess;
        // public string Content;
        public Scan(string ip)
        {
            IP = ip;
            Run();
        }
        private void Run()
        {
            IsSuccess = false;
            try
            {
                WebRequest request = (HttpWebRequest)WebRequest.Create($"http://{IP}");
                WebResponse response = (HttpWebResponse)request.GetResponse();
                //using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                //{
                //    Content = stream.ReadToEnd();
                //}
                response.Close();
                IsSuccess = true;
            }
            catch { }
        }
        public void Save(string sessionName)
        {
            string path = $"{Directories.IPfiles}\\{sessionName}_OUT.csv";
            string info = $"{IP};{DateTime.Now}\n";
            File.AppendAllText(path, info);
        }
    }
}
