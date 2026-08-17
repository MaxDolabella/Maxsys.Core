namespace Chinook.Api.Model.Entities;

/// <summary>
/// Table: CustomerPremium (TPT — herda Customer)
/// </summary>
public sealed class CustomerPremium : Customer
{
    /// <summary>
    /// Column: MemberSince
    /// </summary>
    public DateTime MemberSince { get; set; }

    /// <summary>
    /// Column: DiscountRate
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// Column: MembershipTier
    /// </summary>
    public string? MembershipTier { get; set; }
}
