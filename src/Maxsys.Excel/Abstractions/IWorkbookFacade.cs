using Maxsys.Core.Interfaces.Services;

namespace Maxsys.Excel.Abstractions;

public interface IWorkbookFacade : IService
{
    OperationResult Initialize(Stream file, long? maxFileSizeBytes = 52_428_800);

    OperationResult<IEnumerable<TDestination>?> ReadTable<TDestination>(string? tableName = null) where TDestination : new();

    OperationResult<IEnumerable<TDestination>?> ReadData<TDestination>(int worksheetPosition = 1) where TDestination : new();
}