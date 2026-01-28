namespace Customer.Domain.Enums;

/// <summary>
/// Customer type enumeration.
/// </summary>
public enum CustomerType
{
    /// <summary>Individual customer.</summary>
    Individual = 0,

    /// <summary>Business customer.</summary>
    Business = 1,

    /// <summary>Government customer.</summary>
    Government = 2,

    /// <summary>Non-profit customer.</summary>
    NonProfit = 3,
}

/// <summary>
/// Vendor type enumeration.
/// </summary>
public enum VendorType
{
    /// <summary>Supplier vendor.</summary>
    Supplier = 0,

    /// <summary>Contractor vendor.</summary>
    Contractor = 1,

    /// <summary>Service vendor.</summary>
    Service = 2,

    /// <summary>Utility vendor.</summary>
    Utility = 3,
}

/// <summary>
/// Address type enumeration.
/// </summary>
public enum AddressType
{
    /// <summary>Billing address.</summary>
    Billing = 0,

    /// <summary>Shipping address.</summary>
    Shipping = 1,

    /// <summary>Both billing and shipping address.</summary>
    Both = 2,
}

/// <summary>
/// Payment terms enumeration.
/// </summary>
public enum PaymentTerms
{
    /// <summary>Net 15 days.</summary>
    Net15 = 0,

    /// <summary>Net 30 days.</summary>
    Net30 = 1,

    /// <summary>Net 45 days.</summary>
    Net45 = 2,

    /// <summary>Net 60 days.</summary>
    Net60 = 3,

    /// <summary>Net 90 days.</summary>
    Net90 = 4,

    /// <summary>Due on receipt.</summary>
    DueOnReceipt = 5,

    /// <summary>Custom payment terms.</summary>
    Custom = 6,
}
