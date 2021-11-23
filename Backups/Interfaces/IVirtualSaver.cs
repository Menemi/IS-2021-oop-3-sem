using System.Collections.Generic;

namespace Backups.Interfaces
{
    public interface IVirtualSaver
    {
        public void Save(List<string> files, RestorePoint restorePoint);
    }
}