namespace Chinook.Api.Model.Entities;

/// <summary>
/// Table: InvoiceLine
/// </summary>
public sealed class InvoiceLine
{
    /// <summary>
    /// Column: InvoiceLineId
    /// </summary>
    public int InvoiceLineId { get; set; }

    /// <summary>
    /// Column: InvoiceId
    /// </summary>
    public int InvoiceId { get; set; }

    /// <summary>
    /// Column: TrackId
    /// </summary>
    public int TrackId { get; set; }

    /// <summary>
    /// Column: UnitPrice
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Column: Quantity
    /// </summary>
    public int Quantity { get; set; }

    public Invoice Invoice { get; set; } = null!;

    public Track Track { get; set; } = null!;
}
