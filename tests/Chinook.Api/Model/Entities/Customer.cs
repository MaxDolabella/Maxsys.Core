namespace Chinook.Api.Model.Entities;

/// <summary>
/// Table: Customer
/// </summary>
public class Customer
{
    /// <summary>
    /// Column: CustomerId
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Column: FirstName
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Column: LastName
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Column: Company
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// Column: Address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Column: City
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Column: State
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Column: Country
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Column: PostalCode
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// Column: Phone
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Column: Fax
    /// </summary>
    public string? Fax { get; set; }

    /// <summary>
    /// Column: Email
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Column: SupportRepId
    /// </summary>
    public int? SupportRepId { get; set; }

    public Employee? SupportRep { get; set; }

    public ICollection<Invoice> Invoices { get; set; } = [];
}
