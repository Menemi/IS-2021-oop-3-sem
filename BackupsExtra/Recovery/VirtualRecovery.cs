using System.Collections.Generic;
using System.IO;
using Backups;

namespace BackupsExtra.Recovery
{
    public class VirtualRecovery : IRecoveryProcessMethod
    {
        public void Recovery(RestorePoint restorePoint, List<string> pathsToRecovery)
        {
            var i = 0;
            foreach (var repository in restorePoint.GetRepositories())
            {
                foreach (var file in repository.GetStorageList())
                {
                    var directoryToRecovery = new DirectoryInfo(pathsToRecovery[i]);
                    if (!directoryToRecovery.Exists)
                    {
                        directoryToRecovery.Create();
                    }

                    file.CopyTo(Path.Combine(pathsToRecovery[i], file.Name));
                    ++i;
                }
            }
        }
    }
}