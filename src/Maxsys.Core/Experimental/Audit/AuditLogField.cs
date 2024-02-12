namespace Maxsys.Experimental.Core.Audit;

public class AuditLogField
{
    public required string Field { get; set; }
    public required string? OldValue { get; set; }
    public required string? NewValue { get; set; }
}