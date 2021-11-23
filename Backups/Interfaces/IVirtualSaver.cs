using System.Collections.Generic;
using System.IO;

namespace Backups.Interfaces
{
    public interface IVirtualSaver
    {
        public void Save(List<FileInfo> files, RestorePoint restorePoint);
    }
}