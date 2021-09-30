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
        public void RunScan()
        {
            if (IsScanning || Session == null) return;

            if (IPsList.Count > 0)
            {
                int n = (IPsList.Count / Session.ThreadsCount) + 1;
                for (int i = Session.ScannedIPsCount; i < IPsList.Count; i += n)
                {
                    if (i + n >= IPsList.Count)
                    {
                        n -= i + n - IPsList.Count; // avoid segmentation fault
                    }
                    List<string> ips = IPsList.GetRange(i, n);
                    CreateScanTask(ips);
                }
                IsScanning = true;
            } else System.Windows.MessageBox.Show("IP-адреса не обнаружены");
        }
        public void StopScan()
        {
            if (!IsScanning) return;
            IsScanning = false;
        }
        private async void CreateScanTask(List<string> ips)
        {
            await Task.Run(async () =>
            {
                foreach (string ip in ips)
                {
                    await Task.Delay(15); // for tests
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
                scan.Save();
            }
        }
        private bool OpenFile(Session session)
        {
            try
            {
                IPsList = new List<string>(File.ReadAllLines($"{Directories.IPfiles}\\{session.FileName}"));
                return true;
            }
            catch { return false; }
        }
    }
}
