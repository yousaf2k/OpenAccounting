namespace BuildingBlocks.Common.Constants;

/// <summary>
/// Centralized error codes for the application.
/// </summary>
public static class ErrorCodes
{
    // General errors
    public const string GENERAL_ERROR = "GENERAL_ERROR";
    public const string VALIDATION_FAILED = "VALIDATION_FAILED";
    public const string NOT_FOUND = "NOT_FOUND";
    public const string UNAUTHORIZED = "UNAUTHORIZED";
    public const string FORBIDDEN = "FORBIDDEN";
    public const string CONFLICT = "CONFLICT";

    // Business rule violations
    public const string BUSINESS_RULE_VIOLATION = "BUSINESS_RULE_VIOLATION";
    public const string INVALID_OPERATION = "INVALID_OPERATION";

    // Domain-specific errors
    public const string ENTITY_NOT_FOUND = "ENTITY_NOT_FOUND";
    public const string DUPLICATE_ENTITY = "DUPLICATE_ENTITY";
    public const string INVALID_ENTITY_STATE = "INVALID_ENTITY_STATE";

    // Infrastructure errors
    public const string DATABASE_ERROR = "DATABASE_ERROR";
    public const string EXTERNAL_SERVICE_ERROR = "EXTERNAL_SERVICE_ERROR";
    public const string MESSAGE_BROKER_ERROR = "MESSAGE_BROKER_ERROR";

    // Authentication/Authorization errors
    public const string INVALID_CREDENTIALS = "INVALID_CREDENTIALS";
    public const string TOKEN_EXPIRED = "TOKEN_EXPIRED";
    public const string INSUFFICIENT_PERMISSIONS = "INSUFFICIENT_PERMISSIONS";
}