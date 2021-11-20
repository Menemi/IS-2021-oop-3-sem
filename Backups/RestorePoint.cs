using System.IO;

namespace Backups
{
    public class RestorePoint
    {
        private static int _idCounter = 1;

        private static string _path;

        private int _id;

        public RestorePoint(string restorePointName, string backupPlace)
        {
            _id = _idCounter++;
            _path = backupPlace;

            var directory = new DirectoryInfo(@$"{_path}\{restorePointName}{_id}");

            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        public RestorePoint()
        {
            _id = _idCounter++;
        }

        public int GetId()
        {
            return _id;
        }
    }
}