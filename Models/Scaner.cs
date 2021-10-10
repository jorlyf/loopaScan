using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

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
        public void LoadSession(Session session)
        {
            Session = session;
            if (!OpenFile(Session)) System.Windows.MessageBox.Show("Не удалось открыть файл в сканере");
        }
        public Session GetSession()
        {
            return Session;
        }
        public async void RunScan()
        {
            if (IsScanning || Session == null) return;

            if (IPsList.Count > 0)
            {
                await Task.Run(() =>
                {
                    List<List<string>> list = Generic.SplitStringList(IPsList, Session.ThreadsCount);

                    foreach (List<string> lst in list)
                    {
                        CreateScanTask(lst);
                    }
                });
            } else System.Windows.MessageBox.Show("IP-адреса не обнаружены");
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
                    ScanIP(ip);

                    if (!IsScanning)
                    {
                        break;
                    }
                }
            });
        }
        private void ScanIP(string ip)
        {
            Scan scan = new Scan(ip);
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
                IPsList = new List<string>(File.ReadAllLines($"{Directories.IPfiles}\\{session.FileName}"));
                Session.IPsCount = IPsList.Count;
                return true;
            }
            catch { return false; }
        }
    }
}
