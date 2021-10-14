using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;

namespace loopaScan.Models
{
    public class IPParser
    {
        private List<string> IPs = new List<string>();
        private string Filepath;
        public List<string> Start(string filepath)
        {
            Filepath = filepath;

            if (!string.IsNullOrEmpty(Filepath))
            {
                string[] ipRanges = File.ReadAllLines(Filepath);

                foreach (string ipRange in ipRanges)
                    ConvertRange(ipRange);
            }

            return IPs;
        }
        private void ConvertRange(string rawRange)
        {
            string[] range = rawRange.Split("-");

            long ipStart = BitConverter.ToUInt32(IPAddress.Parse(range[0]).GetAddressBytes().Reverse().ToArray(), 0);
            long ipEnd = BitConverter.ToUInt32(IPAddress.Parse(range[1]).GetAddressBytes().Reverse().ToArray(), 0);

            while (ipStart <= ipEnd)
            {
                IPs.Add(ConvertToAdress(ipStart));
                ipStart++;
            }
        }
        private string ConvertToAdress(long addressInt)
        {
            try
            {
                return IPAddress.Parse(addressInt.ToString()).ToString();
            }
            catch
            {
                System.Windows.MessageBox.Show(addressInt.ToString());
                return null;
            }
        }
    }
}
