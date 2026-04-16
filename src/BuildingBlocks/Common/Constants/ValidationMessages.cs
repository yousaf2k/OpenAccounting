namespace BuildingBlocks.Common.Constants;

/// <summary>
/// Centralized validation message templates.
/// </summary>
public static class ValidationMessages
{
    // General validation messages
    public const string REQUIRED = "{PropertyName} is required.";
    public const string NOT_EMPTY = "{PropertyName} must not be empty.";
    public const string NOT_NULL = "{PropertyName} must not be null.";
    public const string LENGTH = "{PropertyName} must be between {MinLength} and {MaxLength} characters.";
    public const string MIN_LENGTH = "{PropertyName} must be at least {MinLength} characters.";
    public const string MAX_LENGTH = "{PropertyName} must be no more than {MaxLength} characters.";
    public const string EMAIL = "{PropertyName} must be a valid email address.";
    public const string PHONE = "{PropertyName} must be a valid phone number.";
    public const string URL = "{PropertyName} must be a valid URL.";

    // Numeric validation messages
    public const string GREATER_THAN = "{PropertyName} must be greater than {ComparisonValue}.";
    public const string GREATER_THAN_OR_EQUAL = "{PropertyName} must be greater than or equal to {ComparisonValue}.";
    public const string LESS_THAN = "{PropertyName} must be less than {ComparisonValue}.";
    public const string LESS_THAN_OR_EQUAL = "{PropertyName} must be less than or equal to {ComparisonValue}.";
    public const string INCLUSIVE_BETWEEN = "{PropertyName} must be between {From} and {To}.";
    public const string EXCLUSIVE_BETWEEN = "{PropertyName} must be between {From} and {To} (exclusive).";

    // Date validation messages
    public const string PAST_DATE = "{PropertyName} must be a past date.";
    public const string FUTURE_DATE = "{PropertyName} must be a future date.";
    public const string DATE_GREATER_THAN = "{PropertyName} must be after {ComparisonValue}.";
    public const string DATE_LESS_THAN = "{PropertyName} must be before {ComparisonValue}.";

    // Collection validation messages
    public const string MINIMUM_COLLECTION_LENGTH = "{PropertyName} must contain at least {MinLength} items.";
    public const string MAXIMUM_COLLECTION_LENGTH = "{PropertyName} must contain no more than {MaxLength} items.";

    // Custom validation messages
    public const string INVALID_FORMAT = "{PropertyName} has an invalid format.";
    public const string DUPLICATE_VALUE = "{PropertyName} must be unique.";
    public const string INVALID_VALUE = "{PropertyName} contains an invalid value.";
}