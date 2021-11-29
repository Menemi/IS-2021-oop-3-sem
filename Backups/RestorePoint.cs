using System.Collections.Generic;
using System.IO;
using Backups.Exceptions;

namespace Backups
{
    public class RestorePoint
    {
        private static int _idCounter = 1;

        private List<Repository> repositories;

        public RestorePoint(string backupPlace)
        {
            Id = _idCounter++;
            Path = backupPlace;
            repositories = new List<Repository>();
        }

        public int Id { get; }

        public string Path { get; }

        public List<Repository> GetRepositories()
        {
            return repositories;
        }

        public void AddRepository(Repository repository)
        {
            repositories.Add(repository);
        }
    }
}