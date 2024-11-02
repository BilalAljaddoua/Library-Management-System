using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// A utility class containing helper methods to validate different input formats.
/// </summary>
public class clsUtils
{
    /// <summary>
    /// Checks if the input is a valid name containing only letters and spaces.
    /// </summary>
    /// <param name="input">The text to be validated.</param>
    /// <returns>A boolean value indicating whether the input is valid as a name.</returns>
    static public bool IsName(string input)
    {
        return !string.IsNullOrEmpty(input) && Regex.IsMatch(input, @"^[\p{L}\s]+$");
    }

    /// <summary>
    /// Checks if the input DateTime matches the format dd/MM/yyyy.
    /// </summary>
    /// <param name="input">The DateTime to be validated.</param>
    /// <returns>A boolean value indicating whether the input DateTime is in the specified format.</returns>
    static public bool IsDate(DateTime input)
    {
        string formattedDate = input.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        return DateTime.TryParseExact(formattedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }

    /// <summary>
    /// Checks if the input is a valid number.
    /// </summary>
    /// <param name="input">The text to be validated.</param>
    /// <returns>A boolean value indicating whether the input is valid as a number.</returns>
    static public bool IsPositiveNumber(string input)
    {
        double number;
        return (double.TryParse(input, out number)&& number>0);
    }

    /// <summary>
    /// Checks if the input is a valid ISBN number in either ISBN-10 or ISBN-13 format.
    /// </summary>
    /// <param name="isbn">The text to be validated.</param>
    /// <returns>A boolean value indicating whether the input is valid as an ISBN number.</returns>
    static public bool IsValidISBN(string isbn)
    {
        isbn = isbn.Replace("-", "").Replace(" ", "");
        return IsValidISBN10(isbn) || IsValidISBN13(isbn);
    }

    /// <summary>
    /// Checks if the input is a valid ISBN-10 number.
    /// </summary>
    /// <param name="isbn">The text to be validated.</param>
    /// <returns>A boolean value indicating whether the input is valid as an ISBN-10 number.</returns>
    static bool IsValidISBN10(string isbn)
    {
        if (isbn.Length != 10 || !isbn.All(char.IsDigit))
            return false;

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (isbn[i] - '0') * (10 - i);
        }

        char lastChar = isbn[9];
        int lastValue = lastChar == 'X' ? 10 : lastChar - '0';
        sum += lastValue;

        return sum % 11 == 0;
    }

    /// <summary>
    /// Checks if the input is a valid ISBN-13 number.
    /// </summary>
    /// <param name="isbn">The text to be validated.</param>
    /// <returns>A boolean value indicating whether the input is valid as an ISBN-13 number.</returns>
    static bool IsValidISBN13(string isbn)
    {
        if (isbn.Length != 13 || !isbn.All(char.IsDigit))
            return false;

        int sum = 0;
        for (int i = 0; i < 13; i++)
        {
            int digit = isbn[i] - '0';
            sum += i % 2 == 0 ? digit : digit * 3;
        }

        return sum % 10 == 0;
    }



    // Method to validate the email
    public static bool IsValidEmail(string email)
    {
          string EmailPattern =   @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        // Check if the email matches the regex pattern
        return Regex.IsMatch(email, EmailPattern);
    }



}
