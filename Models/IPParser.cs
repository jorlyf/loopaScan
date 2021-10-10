using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;
using System.Net;
using System.Linq;

using loopaScan.Infrastructure;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace loopaScan.Models
{
    public class IPParser : INotifyPropertyChanged
    {
        #region Notifier
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion

        private List<string> IPs = new List<string>();
        private string _Filepath;
        public string Filepath
        {
            get { return _Filepath; }
            set
            {
                _Filepath = value;
                OnPropertyChanged();
            }
        }
        public bool Start()
        {
            if (!string.IsNullOrEmpty(Filepath))
            {
                string[] ipRanges = File.ReadAllLines(Filepath);

                foreach (string ipRange in ipRanges)
                    ConvertRange(ipRange);

                Save();
                return true;
            }

            return false;
        }
        public bool Load()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = ".txt|*.txt",
            };

            if (ofd.ShowDialog() == false) return false;

            Filepath = ofd.FileName;
            return true;
        }
        private void ConvertRange(string rawRange)
        {
            string[] range = rawRange.Split("-");

            long ipStart = BitConverter.ToUInt32(IPAddress.Parse(range[0]).GetAddressBytes().Reverse().ToArray(), 0);
            long ipEnd = BitConverter.ToUInt32(IPAddress.Parse(range[1]).GetAddressBytes().Reverse().ToArray(), 0);

            System.Windows.MessageBox.Show($"{ipStart} - {ipEnd}\n{rawRange}");

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
        private bool Save()
        {
            try
            {
                File.WriteAllLines($"{Filepath.Remove(Filepath.Length - 4)}_OUT.txt", IPs);
                return true;
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
                return false;
            }
        }
    }
}
