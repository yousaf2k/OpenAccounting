namespace BuildingBlocks.Common.Extensions;

/// <summary>
/// Extension methods for DateTime operations.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Converts the DateTime to Unix timestamp (seconds since 1970-01-01).
    /// </summary>
    /// <param name="dateTime">The DateTime to convert.</param>
    /// <returns>The Unix timestamp.</returns>
    public static long ToUnixTimestamp(this DateTime dateTime)
    {
        return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }

    /// <summary>
    /// Converts Unix timestamp to DateTime.
    /// </summary>
    /// <param name="unixTimestamp">The Unix timestamp.</param>
    /// <returns>The DateTime.</returns>
    public static DateTime FromUnixTimestamp(this long unixTimestamp)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimestamp);
    }

    /// <summary>
    /// Gets the start of the day for the DateTime.
    /// </summary>
    /// <param name="dateTime">The DateTime.</param>
    /// <returns>The start of the day.</returns>
    public static DateTime StartOfDay(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, dateTime.Kind);
    }

    /// <summary>
    /// Gets the end of the day for the DateTime.
    /// </summary>
    /// <param name="dateTime">The DateTime.</param>
    /// <returns>The end of the day.</returns>
    public static DateTime EndOfDay(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999, dateTime.Kind);
    }

    /// <summary>
    /// Gets the start of the week for the DateTime (Monday).
    /// </summary>
    /// <param name="dateTime">The DateTime.</param>
    /// <returns>The start of the week.</returns>
    public static DateTime StartOfWeek(this DateTime dateTime)
    {
        int diff = (7 + (dateTime.DayOfWeek - DayOfWeek.Monday)) % 7;
        return dateTime.AddDays(-1 * diff).Date;
    }

    /// <summary>
    /// Gets the end of the week for the DateTime (Sunday).
    /// </summary>
    /// <param name="dateTime">The DateTime.</param>
    /// <returns>The end of the week.</returns>
    public static DateTime EndOfWeek(this DateTime dateTime)
    {
        return dateTime.StartOfWeek().AddDays(6).EndOfDay();
    }

    /// <summary>
    /// Gets the start of the month for the DateTime.
    /// </summary>
    /// <param name="dateTime">The DateTime.</param>
    /// <returns>The start of the month.</returns>
    public static DateTime StartOfMonth(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, dateTime.Kind);
    }

    /// <summary>
    /// Gets the end of the month for the DateTime.
    /// </summary>
    /// <param name="dateTime">The DateTime.</param>
    /// <returns>The end of the month.</returns>
    public static DateTime EndOfMonth(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month), 23, 59, 59, 999, dateTime.Kind);
    }

    /// <summary>
    /// Checks if the DateTime is between two other DateTimes.
    /// </summary>
    /// <param name="dateTime">The DateTime to check.</param>
    /// <param name="start">The start DateTime.</param>
    /// <param name="end">The end DateTime.</param>
    /// <returns>true if the DateTime is between start and end; otherwise, false.</returns>
    public static bool IsBetween(this DateTime dateTime, DateTime start, DateTime end)
    {
        return dateTime >= start && dateTime <= end;
    }

    /// <summary>
    /// Gets the age in years from the DateTime to now.
    /// </summary>
    /// <param name="dateTime">The birth date.</param>
    /// <returns>The age in years.</returns>
    public static int GetAge(this DateTime dateTime)
    {
        var today = DateTime.Today;
        var age = today.Year - dateTime.Year;
        if (dateTime.Date > today.AddYears(-age)) age--;
        return age;
    }
}