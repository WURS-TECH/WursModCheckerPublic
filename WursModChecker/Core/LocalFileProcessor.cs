using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WursModChecker.Core.Contracts;

namespace WursModChecker.Core
{
    public class LocalFileProcessor : ILocalFileProcessor
    {
        public LocalFileProcessor()
        {
        }

        public async Task<IDictionary<string, byte[]>> CreateLocalModsInMemoryBackup(string localModsFolderPath)
        {
            DirectoryInfo source = new(localModsFolderPath);
            return IsDirectoryExist(localModsFolderPath) ?
                await ConvertFilesToBackupDictionary(source.GetFiles("*.*", SearchOption.AllDirectories)) :
                new Dictionary<string, byte[]>();
        }

        public async Task RestoreFromBackup(IDictionary<string, byte[]> backup, string localModsFolderPath)
        {
            DeleteCorruptedDirectory(localModsFolderPath);
            Directory.CreateDirectory(localModsFolderPath);
            foreach (var pairBackupData in backup)
            {
                await File.WriteAllBytesAsync(localModsFolderPath + $@"\{pairBackupData.Key}", pairBackupData.Value);
            }
        }
        private static async Task<IDictionary<string, byte[]>> ConvertFilesToBackupDictionary(IEnumerable<FileInfo> localModList)
        {
            Dictionary<string, byte[]> backupDictionary = new();
            foreach (var modFile in localModList)
            {
                if (!string.IsNullOrWhiteSpace(modFile.FullName))
                    backupDictionary.Add(modFile.Name, await File.ReadAllBytesAsync(modFile.FullName));
            }
            return backupDictionary;
        }
        private static void DeleteCorruptedDirectory(string path)
        {
            if (IsDirectoryExist(path))
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(path,
                    Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                    Microsoft.VisualBasic.FileIO.RecycleOption.DeletePermanently);
        }

        private static bool IsDirectoryExist(string localModsFolderPath)
        {
            return Directory.Exists(localModsFolderPath);
        }
    }
}
