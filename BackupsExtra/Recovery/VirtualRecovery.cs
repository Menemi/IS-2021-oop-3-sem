using System.Collections.Generic;
using System.IO;
using Backups;

namespace BackupsExtra.Recovery
{
    public class VirtualRecovery : IRecoveryType
    {
        public void Recovery(RestorePoint restorePoint, List<string> pathsToRecovery)
        {
            foreach (var repository in restorePoint.GetRepositories())
            {
                foreach (var file in repository.GetStorageList())
                {
                    var fileInfo = new FileInfo(file.FullName);
                    fileInfo.CopyTo(Path.Combine(pathsToRecovery[0], fileInfo.Name));
                }
            }
        }
    }
}