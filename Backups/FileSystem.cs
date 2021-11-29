using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Backups.Exceptions;

namespace Backups
{
    public class FileSystem
    {
        private List<DirectoryInfo> _restorePointDirectories;

        public FileSystem(string generalPath)
        {
            GeneralPath = generalPath;
            _restorePointDirectories = new List<DirectoryInfo>();
        }

        public string GeneralPath { get; }

        public ReadOnlyCollection<DirectoryInfo> GetRestorePointDirectories()
        {
            return _restorePointDirectories.AsReadOnly();
        }

        public DirectoryInfo AddRestorePointDirectory(string restorePointPath)
        {
            var directory = new DirectoryInfo(restorePointPath);
            if (directory.Exists)
            {
                throw new BackupsException("Directory already exists");
            }

            directory.Create();

            _restorePointDirectories.Add(directory);
            return directory;
        }
    }
}