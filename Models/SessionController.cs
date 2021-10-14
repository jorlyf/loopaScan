using System.Collections.ObjectModel;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

using loopaScan.Infrastructure;

namespace loopaScan.Models
{
    public class SessionController
    {
        public ObservableCollection<Session> GetSessions()
        {
            ObservableCollection<Session> sessions = new ObservableCollection<Session>();
            foreach (string jsonPath in Directory.EnumerateFiles(Directories.Sessions, "*.json"))
            {
                string file = File.ReadAllText(jsonPath);
                sessions.Add(JsonConvert.DeserializeObject<Session>(file));
            }
            return sessions;
        }
        public bool SaveSessions(ObservableCollection<Session> sessions)
        {
            foreach (Session session in sessions)
            {
                if (!session.Save())
                    System.Windows.MessageBox.Show($"Не вышло сохранить сессию с именем {session.Name}");
            }
            return true;
        }
        
    }

    public class Session
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public int IPsCount { get; set; }
        public int ScannedIPsCount { get; set; }
        public int ScannedSuccessIPsCount { get; set; }
        public int ThreadsCount { get; set; }
        public List<string> Ports { get; set; }

        public bool Save()
        {
            try
            {
                File.WriteAllText($"{Directories.Sessions}\\{Name}.json", JsonConvert.SerializeObject(this));
                return true;
            }
            catch { return false; }
        }
        public bool Delete()
        {
            try
            {
                string path = $"{Directories.Sessions}\\{Name}.json";
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            }
            catch
            {
                System.Windows.MessageBox.Show("Сессия не была удалена");
                return false;
            }
        }
    }
}
