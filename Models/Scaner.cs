using System.Collections.Generic;
using System.Threading.Tasks;

using loopaScan.Infrastructure;

namespace loopaScan.Models
{
    class Scaner
    {
        private Session Session;
        public bool IsScanning
        {
            get;
            private set;
        }
        private List<string> IPsList;
        private IPParser IPParser = new IPParser();
        public void LoadSession(Session session)
        {
            Session = session;
            if (!OpenFile(Session)) System.Windows.MessageBox.Show("Не удалось открыть файл в сканере");
        }
        public async void RunScan()
        {
            if (IsScanning || Session == null) return;

            if (IPsList.Count > 0)
            {
                await Task.Run(() =>
                {
                    IsScanning = true;
                    IPsList.RemoveRange(0, Session.ScannedIPsCount); // delete scanned IPs
                    List<List<string>> list = Generic.SplitStringList(IPsList, Session.ThreadsCount);

                    foreach (List<string> lst in list)
                    {
                        CreateScanTask(lst);
                    }
                });
            }
            else System.Windows.MessageBox.Show("IP-адреса не обнаружены");
        }
        public void StopScan()
        {
            if (!IsScanning) return;
            IsScanning = false;
        }
        private async void CreateScanTask(List<string> ips)
        {
            await Task.Run(() =>
            {
                foreach (string ip in ips)
                {
                    foreach (string port in Session.Ports)
                    {
                        if (!IsScanning) return;
                        ScanIP(ip, port);
                    }
                }
            });
        }
        private void ScanIP(string ip, string port)
        {
            Scan scan = new Scan(ip, port);
            if (!IsScanning) return;

            Session.ScannedIPsCount++;
            if (scan.IsSuccess)
            {
                Session.ScannedSuccessIPsCount++;
                scan.Save(Session.Name);
            }
        }
        private bool OpenFile(Session session)
        {
            try
            {
                IPsList = ParseRange($"{Directories.IPfiles}\\{session.FileName}");
                Session.IPsCount = IPsList.Count;
                return true;
            }
            catch { return false; }
        }
        private List<string> ParseRange(string filepath)
        {
            var result = IPParser.Start(filepath);
            return result;
        }
    }
}