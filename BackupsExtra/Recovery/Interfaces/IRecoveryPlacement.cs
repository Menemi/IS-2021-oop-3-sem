using Backups;

namespace BackupsExtra.Recovery
{
    public interface IRecoveryPlacement
    {
        // original / custom
        void Recovery(IRecoveryProcessMethod recoveryProcessMethod, RestorePoint restorePoint, string pathToRecovery);
    }
}