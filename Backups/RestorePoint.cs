using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            CreationTime = DateTime.Now;
        }

        public int Id { get; }

        public string Path { get; }

        public DateTime CreationTime { get; }

        public ReadOnlyCollection<Repository> GetRepositories()
        {
            return repositories.AsReadOnly();
        }

        public void AddRepository(Repository repository)
        {
            repositories.Add(repository);
        }
    }
}