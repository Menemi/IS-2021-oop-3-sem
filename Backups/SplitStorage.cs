using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;

namespace Backups
{
    public class SplitStorage : IVirtualSaver
    {
        public void Save(List<string> files, RestorePoint restorePoint)
        {
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                var repository = restorePoint.AddRepository();
                repository.AddFileToStorage(fileInfo);
            }
        }
    }
}