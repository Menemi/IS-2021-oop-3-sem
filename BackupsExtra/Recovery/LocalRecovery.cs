using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups;

namespace BackupsExtra.Recovery
{
    public class LocalRecovery : IRecoveryType
    {
        public void Recovery(RestorePoint restorePoint, List<string> pathsToRecovery)
        {
            var i = 0;
            var zipFileCounter = 0;
            foreach (var repository in restorePoint.GetRepositories())
            {
                zipFileCounter++;
                var tempDirectory = new DirectoryInfo(Path.Combine($"{restorePoint.Path}{restorePoint.Id}", "temp"));
                tempDirectory.Create();
                ZipFile.ExtractToDirectory(
                    Path.Combine($"{restorePoint.Path}{restorePoint.Id}", $"Files_{zipFileCounter}.zip"),
                    tempDirectory.FullName);
                foreach (var file in tempDirectory.GetFileSystemInfos())
                {
                    i++;
                    var fileInfo = new FileInfo(file.FullName);
                    fileInfo.CopyTo(Path.Combine(pathsToRecovery[0], fileInfo.Name));
                }

                tempDirectory.Delete(true);
            }
        }
    }
}