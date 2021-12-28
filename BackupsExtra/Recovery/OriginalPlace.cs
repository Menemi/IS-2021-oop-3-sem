using System.Collections.Generic;
using System.Linq;
using Backups;

namespace BackupsExtra.Recovery
{
    public class OriginalPlace : IRecoveryPlace
    {
        public void Recovery(IRecoveryType recoveryType, RestorePoint restorePoint, string pathToRecovery)
        {
            var pathsToRecovery = new List<string>();
            pathsToRecovery.AddRange(
                from repository in restorePoint.GetRepositories()
                from file in repository.GetStorageList()
                select file.Directory.ToString());

            recoveryType.Recovery(restorePoint, pathsToRecovery);
        }
    }
}