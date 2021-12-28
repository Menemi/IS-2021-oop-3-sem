using System.Collections.Generic;
using Backups;

namespace BackupsExtra.Recovery
{
    public interface IRecoveryType
    {
        // local / virtual
        void Recovery(RestorePoint restorePoint, List<string> pathsToRecovery);
    }
}