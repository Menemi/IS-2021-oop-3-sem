using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups;

namespace BackupsExtra.Merge
{
    public class LocalMerge : IMergeProcessMethod
    {
        public RestorePoint Merge(
            ComplementedBackupJob backupJob,
            RestorePoint oldRestorePoint,
            RestorePoint newRestorePoint,
            bool isTimecodeOn)
        {
            if (oldRestorePoint.GetRepositories().Count == 1 ||
                newRestorePoint.GetRepositories().Count == 1)
            {
                if (oldRestorePoint.GetRepositories()[0].GetStorageList().Count != 1 ||
                    newRestorePoint.GetRepositories()[0].GetStorageList().Count != 1)
                {
                    var restorePointsToRemove = new List<RestorePoint> { oldRestorePoint };
                    var restorePointDirectoryToRemove =
                        new DirectoryInfo($"{oldRestorePoint.Path}{oldRestorePoint.Id}");
                    restorePointDirectoryToRemove.Delete(true);
                    backupJob.RemoveRestorePoints(restorePointsToRemove, isTimecodeOn);
                    return newRestorePoint;
                }
            }

            var newRepositories = new List<Repository>();
            var oldZipFileCounter = 0;
            var oldTempDirectory =
                new DirectoryInfo(Path.Combine($"{oldRestorePoint.Path}{oldRestorePoint.Id}", "oldTemp"));
            foreach (var oldRepository in oldRestorePoint.GetRepositories())
            {
                oldZipFileCounter++;

                ZipFile.ExtractToDirectory(
                    Path.Combine($"{oldRestorePoint.Path}{oldRestorePoint.Id}", $"Files_{oldZipFileCounter}.zip"),
                    oldTempDirectory.FullName);
            }

            var newZipFileCounter = 0;
            var newTempDirectory =
                new DirectoryInfo(Path.Combine($"{newRestorePoint.Path}{newRestorePoint.Id}", "newTemp"));
            foreach (var newRepository in newRestorePoint.GetRepositories())
            {
                newZipFileCounter++;

                ZipFile.ExtractToDirectory(
                    Path.Combine($"{newRestorePoint.Path}{newRestorePoint.Id}", $"Files_{newZipFileCounter}.zip"),
                    newTempDirectory.FullName);
            }

            foreach (var oldFile in oldTempDirectory.GetFileSystemInfos())
            {
                var oldFileInfo = new FileInfo(oldFile.FullName);
                var checkingAvailabilityFlag = false;
                var repository = new Repository();

                foreach (var newFile in newTempDirectory.GetFileSystemInfos())
                {
                    if (oldFile.Name == newFile.Name)
                    {
                        var newFileInfo = new FileInfo(newFile.FullName);
                        repository.AddStorage(newFileInfo);
                        newRepositories.Add(repository);
                        checkingAvailabilityFlag = true;
                    }
                }

                if (!checkingAvailabilityFlag)
                {
                    repository.AddStorage(oldFileInfo);
                    newRepositories.Add(repository);
                }
            }

            var repositoriesToDelete = newRestorePoint.GetRepositories();
            newRestorePoint.RemoveRepositories(repositoriesToDelete.ToList());
            var directoryToDelete = new DirectoryInfo($"{newRestorePoint.Path}{newRestorePoint.Id}");
            foreach (var file in directoryToDelete.GetFiles())
            {
                file.Delete();
            }

            var tempDirectory = new DirectoryInfo(Path.Combine($"{newRestorePoint.Path}{newRestorePoint.Id}", "temp"));
            tempDirectory.Create();
            var id = 0;
            foreach (var repository in newRepositories)
            {
                newRestorePoint.AddRepository(repository);
                foreach (var fileToAdd in repository.GetStorageList())
                {
                    fileToAdd.CopyTo(Path.Combine(tempDirectory.FullName, fileToAdd.Name));
                }

                ++id;
                ZipFile.CreateFromDirectory(
                    tempDirectory.FullName,
                    Path.Combine($"{newRestorePoint.Path}{newRestorePoint.Id}", $"Files_{id}.zip"));
                foreach (var file in tempDirectory.GetFiles())
                {
                    file.Delete();
                }
            }

            var oldRestorePoints = new List<RestorePoint> { oldRestorePoint };
            var oldRestorePointDirectory = new DirectoryInfo($"{oldRestorePoint.Path}{oldRestorePoint.Id}");
            oldRestorePointDirectory.Delete(true);
            backupJob.RemoveRestorePoints(oldRestorePoints, isTimecodeOn);
            newTempDirectory.Delete(true);
            tempDirectory.Delete(true);

            return newRestorePoint;
        }
    }
}