using System.Collections.Generic;
using System.IO;

namespace Backups
{
    public class Repository
    {
        public Repository()
        {
            Storages = new List<FileInfo>();
        }

        public List<FileInfo> Storages { get; }

        public void AddFileToStorage(FileInfo file)
        {
            Storages.Add(file);
        }
    }
}