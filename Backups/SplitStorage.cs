using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups
{
    public class SplitStorage : IVirtualSaver
    {
        public void Save(List<FileInfo> files, RestorePoint restorePoint)
        {
            foreach (var file in files)
            {
                var repository = restorePoint.AddRepository();
                repository.AddFileToStorage(file);
            }
        }
    }
}