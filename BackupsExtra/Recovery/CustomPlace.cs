using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups;

namespace BackupsExtra.Recovery
{
    public class CustomPlace : IRecoveryPlace
    {
        public void Recovery(IRecoveryType recoveryType, RestorePoint restorePoint, string pathToRecovery)
        {
            var pathsToRecovery = (
                from repository in restorePoint.GetRepositories()
                from file in repository.GetStorageList()
                select pathToRecovery).ToList();

            recoveryType.Recovery(restorePoint, pathsToRecovery);
        }
    }
}