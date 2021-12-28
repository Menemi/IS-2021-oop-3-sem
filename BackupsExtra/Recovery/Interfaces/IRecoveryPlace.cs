using System.Collections.Generic;
using Backups;

namespace BackupsExtra.Recovery
{
    public interface IRecoveryPlace
    {
        // original / custom
        void Recovery(IRecoveryType recoveryType, RestorePoint restorePoint, string pathToRecovery);
    }
}