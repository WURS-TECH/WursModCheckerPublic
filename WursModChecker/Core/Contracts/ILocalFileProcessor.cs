using System.Collections.Generic;
using System.Threading.Tasks;

namespace WursModChecker.Core.Contracts
{
    public interface ILocalFileProcessor
    {
        Task<IDictionary<string, byte[]>> CreateLocalModsInMemoryBackup(string localModsFolderPath);

        Task RestoreFromBackup(IDictionary<string, byte[]> backup, string localModsFolderPath);
    }
}
