using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace SecurityAndAuth.Utils;

public interface IPasswordValidator
{
    bool IsValidPassword(string password);
    string PreventXss(string input);
}

public partial class PasswordValidator : IPasswordValidator
{
    private static readonly HashSet<string> CommonPasswords =
    [
        "password",
        "123456",
        "qwerty",
        "letmein",
        "welcome",
        "passw0rd",
    ];

    public bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            return false;

        return !CommonPasswords.Contains(password.ToLower());
    }

    public string PreventXss(string input)
    {
        return HttpUtility.HtmlEncode(input);
    }

    public static string SqlEscape(string input)
    {
        // Replace single quotes with two single quotes (escaping them)
        return input.Replace("'", "''");
    }
}
