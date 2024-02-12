using Maxsys.Core.Interfaces.Services;

namespace Maxsys.Core.Excel.Abstractions;

public interface IWorkbookFacade : IService
{
    OperationResult Initialize(Stream file);

    OperationResult<IEnumerable<TDestination>?> ReadTable<TDestination>(string? tableName = null);

    OperationResult<IEnumerable<TDestination>?> ReadData<TDestination>(int worksheetPosition = 1);
}