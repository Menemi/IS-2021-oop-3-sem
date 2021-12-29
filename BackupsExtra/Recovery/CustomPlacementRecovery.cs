using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups;

namespace BackupsExtra.Recovery
{
    public class CustomPlacementRecovery : IRecoveryPlacement
    {
        public void Recovery(IRecoveryProcessMethod recoveryProcessMethod, RestorePoint restorePoint, string pathToRecovery)
        {
            var pathsToRecovery = new List<string>();
            foreach (var repository in restorePoint.GetRepositories())
            {
                foreach (var file in repository.GetStorageList())
                {
                    pathsToRecovery.Add(pathToRecovery);
                }
            }

            recoveryProcessMethod.Recovery(restorePoint, pathsToRecovery);
        }
    }
}