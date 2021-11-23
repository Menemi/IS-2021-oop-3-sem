using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups
{
    public class SingleStorage : IVirtualSaver
    {
        public void Save(List<FileInfo> files, RestorePoint restorePoint)
        {
            var repository = restorePoint.AddRepository();
            foreach (var file in files)
            {
                repository.AddFileToStorage(file);
            }
        }
    }
}