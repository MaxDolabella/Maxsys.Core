using System.Text.Json.Serialization;

namespace Maxsys.Core.DTO;

public abstract class MonitorableDTO : IDTO
{
    [JsonPropertyOrder(int.MaxValue)]
    public UpdateStatus UpdateStatus { get; set; } = UpdateStatus.Loaded;
}