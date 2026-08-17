namespace Chinook.Api.Model.Entities;

/// <summary>
/// Table: Invoice
/// </summary>
public sealed class Invoice
{
    /// <summary>
    /// Column: InvoiceId
    /// </summary>
    public int InvoiceId { get; set; }

    /// <summary>
    /// Column: CustomerId
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Column: InvoiceDate
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// Column: BillingAddress
    /// </summary>
    public string? BillingAddress { get; set; }

    /// <summary>
    /// Column: BillingCity
    /// </summary>
    public string? BillingCity { get; set; }

    /// <summary>
    /// Column: BillingState
    /// </summary>
    public string? BillingState { get; set; }

    /// <summary>
    /// Column: BillingCountry
    /// </summary>
    public string? BillingCountry { get; set; }

    /// <summary>
    /// Column: BillingPostalCode
    /// </summary>
    public string? BillingPostalCode { get; set; }

    /// <summary>
    /// Column: Total
    /// </summary>
    public decimal Total { get; set; }

    public Customer Customer { get; set; } = null!;

    public ICollection<InvoiceLine> InvoiceLines { get; set; } = [];
}
