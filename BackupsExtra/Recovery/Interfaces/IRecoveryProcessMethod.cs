using System.Collections.Generic;
using Backups;

namespace BackupsExtra.Recovery
{
    public interface IRecoveryProcessMethod
    {
        // local / virtual
        void Recovery(RestorePoint restorePoint, List<string> pathsToRecovery);
    }
}