using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public void RemoveRepositories(List<Repository> repository)
        {
            foreach (var rep in repository)
            {
                if (repositories.Contains(rep))
                {
                    repositories.Remove(rep);
                }
                else
                {
                    throw new BackupsException("You can't remove non-existent repository");
                }
            }
        }
    }
}