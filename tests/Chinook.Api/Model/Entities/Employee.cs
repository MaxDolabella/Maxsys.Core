namespace Chinook.Api.Model.Entities;

/// <summary>
/// Table: Employee
/// </summary>
public sealed class Employee
{
    /// <summary>
    /// Column: EmployeeId
    /// </summary>
    public int EmployeeId { get; set; }

    /// <summary>
    /// Column: LastName
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Column: FirstName
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Column: Title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Column: ReportsTo
    /// </summary>
    public int? ReportsTo { get; set; }

    /// <summary>
    /// Column: BirthDate
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Column: HireDate
    /// </summary>
    public DateTime? HireDate { get; set; }

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
    public string? Email { get; set; }

    public Employee? Manager { get; set; }

    public ICollection<Employee> DirectReports { get; set; } = [];

    public ICollection<Customer> Customers { get; set; } = [];
}
