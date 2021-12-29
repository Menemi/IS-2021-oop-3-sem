using System.Collections.Generic;
using System.Linq;
using Backups;

namespace BackupsExtra.Recovery
{
    public class OriginalPlacementRecovery : IRecoveryPlacement
    {
        public void Recovery(IRecoveryProcessMethod recoveryProcessMethod, RestorePoint restorePoint, string pathToRecovery)
        {
            var pathsToRecovery = new List<string>();
            foreach (var repository in restorePoint.GetRepositories())
            {
                foreach (var file in repository.GetStorageList())
                {
                    pathsToRecovery.Add(file.Directory.ToString());
                }
            }

            recoveryProcessMethod.Recovery(restorePoint, pathsToRecovery);
        }
    }
}