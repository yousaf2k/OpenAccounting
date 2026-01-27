namespace BuildingBlocks.Common.Extensions;

/// <summary>
/// Extension methods for string operations.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Checks if the string is null or empty.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>true if the string is null or empty; otherwise, false.</returns>
    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Checks if the string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>true if the string is null, empty, or consists only of white-space characters; otherwise, false.</returns>
    public static bool IsNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Converts the string to title case.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The string converted to title case.</returns>
    public static string ToTitleCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
    }

    /// <summary>
    /// Truncates the string to the specified maximum length.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="maxLength">The maximum length of the string.</param>
    /// <returns>The truncated string.</returns>
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            return value;

        return value.Substring(0, maxLength);
    }

    /// <summary>
    /// Converts the string to a slug (URL-friendly format).
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The string converted to a slug.</returns>
    public static string ToSlug(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        // Replace spaces and special characters with hyphens
        var slug = System.Text.RegularExpressions.Regex.Replace(value.ToLower(), @"[^a-z0-9\s-]", "");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-");

        return slug.Trim('-');
    }
}