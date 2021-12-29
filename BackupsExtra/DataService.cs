using System;
using System.Collections.Generic;
using System.IO;
using BackupsExtra.Logging;
using Newtonsoft.Json;

namespace BackupsExtra
{
    public class DataService
    {
        private List<ComplementedBackupJob> _backupJobs;

        private ILogging _logger;

        public DataService(ILogging logger)
        {
            _backupJobs = new List<ComplementedBackupJob>();
            _logger = logger;
        }

        public void AddBackupJob(ComplementedBackupJob backupJob)
        {
            _backupJobs.Add(backupJob);
        }

        public void SaveData(bool isTimecodeOn)
        {
            File.WriteAllText(
                "D:/ITMOre than a university/1Menemi1/BackupsExtra/data.json",
                JsonConvert.SerializeObject(_backupJobs));

            _logger.CreateLog(isTimecodeOn, "Serialize process was done successfully");
        }

        public void LoadData(bool isTimecodeOn)
        {
            _backupJobs = JsonConvert.DeserializeObject<List<ComplementedBackupJob>>(
                File.ReadAllText("D:/ITMOre than a university/1Menemi1/BackupsExtra/data.json"));

            _logger.CreateLog(isTimecodeOn, "Deserialize process was done successfully");
        }
    }
}