using System.IO;

namespace loopaScan.Infrastructure
{
    internal static class Directories
    {
        public static string Root { get => Directory.GetCurrentDirectory(); }
        public static string IPfiles { get => $"{Root}\\IPfiles"; }
        public static string Sessions { get => $"{Root}\\Sessions"; }


        public static void CreateDirectoriesOnStartup()
        {
            CreateIPsDirectory();
            CreateSessionsDirectory();
        }
        private static void CreateIPsDirectory()
        {
            if (!Directory.Exists(IPfiles))
                Directory.CreateDirectory(IPfiles);
        }
        private static void CreateSessionsDirectory()
        {
            if (!Directory.Exists(Sessions))
                Directory.CreateDirectory(Sessions);
        }
    }
}
