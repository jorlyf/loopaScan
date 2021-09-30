using System.Net;
using System.IO;

namespace loopaScan.Models
{
    class Scan
    {
        public string IP;
        public bool IsSuccess;
        public string Response;
        public Scan(string ip)
        {
            IP = ip;
            Run();
        }
        private void Run()
        {
            try
            {
                WebRequest request = (HttpWebRequest)WebRequest.Create($"http://{IP}");
                WebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string content = reader.ReadToEnd();
                    }
                }
                response.Close();
            }
            catch (WebException)
            {
                
            }
        }
        public void Save()
        {

        }
    }
}
