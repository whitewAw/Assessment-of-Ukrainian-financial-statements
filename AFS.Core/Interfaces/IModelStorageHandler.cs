using AFS.Core.Models;

namespace AFS.Core.Interfaces
{
    public interface IModelStorageHandler
    {
        Task WriteModelAsync(AfsModel model);
        Task<AfsModel?> ReadModelAsync();
        Task InitializeModelAsync();
    }
}
