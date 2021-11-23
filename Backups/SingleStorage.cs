using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;

namespace Backups
{
    public class SingleStorage : IVirtualSaver
    {
        public void Save(List<string> files, RestorePoint restorePoint)
        {
            var repository = restorePoint.AddRepository();
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                repository.AddFileToStorage(fileInfo);
            }
        }
    }
}