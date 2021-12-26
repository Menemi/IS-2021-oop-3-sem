using System;
using System.Collections.Generic;
using System.IO;
using Backups.Exceptions;
using Newtonsoft.Json;

namespace BackupsExtra
{
    public class DataService
    {
        private List<ComplementedBackupJob> _backupJobs;

        public DataService()
        {
            _backupJobs = new List<ComplementedBackupJob>();
        }

        public void AddBackupJob(ComplementedBackupJob backupJob)
        {
            _backupJobs.Add(backupJob);
        }

        public void SaveData()
        {
            Console.WriteLine("Serialize process is going...");
            File.WriteAllText(
                "D:/ITMOre than a university/1Menemi1/BackupsExtra/data.json",
                JsonConvert.SerializeObject(_backupJobs));
            Console.WriteLine("All is OK!");
        }

        public void LoadData()
        {
            Console.WriteLine("Deserialize process is going...");
            _backupJobs = JsonConvert.DeserializeObject<List<ComplementedBackupJob>>(
                File.ReadAllText("D:/ITMOre than a university/1Menemi1/BackupsExtra/data.json"));

            // var obj = JsonConvert.DeserializeObject(
            //     File.ReadAllText("D:/ITMOre than a university/1Menemi1/BackupsExtra/data.json"));

            // _backupJobs.Add(obj as ComplementedBackupJob);

            // _backupJobs = obj as List<ComplementedBackupJob>;
            Console.WriteLine("All is OK!");
        }
    }
}