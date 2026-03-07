using AFS.Core.Models;

namespace AFS.Core.Interfaces
{
    public interface IModelExportImportHandler
    {
        Task ExportAsync(AfsModel model);
        Task<AfsModel?> ImportAsync(Stream inputStream);
        void InitializeModel(AfsModel? model);
    }
}
