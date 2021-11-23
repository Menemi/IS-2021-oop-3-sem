using System.Collections.Generic;
using System.IO;

namespace Backups
{
    public class RestorePoint
    {
        private static int _idCounter = 1;

        private static string _path;

        private List<Repository> _repositories;

        public RestorePoint(string restorePointName, string backupPlace)
        {
            Id = _idCounter++;
            _path = backupPlace;
            _repositories = new List<Repository>();

            var directory = new DirectoryInfo(@$"{_path}\{restorePointName}{Id}");

            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        public int Id { get; }

        public List<Repository> GetRepositories()
        {
            return _repositories;
        }

        public Repository AddRepository()
        {
            var repository = new Repository();
            _repositories.Add(repository);
            return repository;
        }
    }
}