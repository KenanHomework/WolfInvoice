using System.Text.RegularExpressions;

namespace WolfInvoice.DTOs.Validation.Shared;

/// <summary>
/// Provides shared validation methods for various types of input.
/// </summary>
public class SharedValidators
{
    /// <summary>
    /// Validates a password to ensure it meets certain complexity requirements.
    /// </summary>
    /// <param name="arg">The password to validate.</param>
    /// <returns>True if the password meets the complexity requirements, otherwise false.</returns>
    public static bool BeValidatePassword(string arg) =>
        new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$").IsMatch(arg);

    /// <summary>
    /// Validates a username to ensure it meets certain requirements.
    /// </summary>
    /// <param name="arg">The username to validate.</param>
    /// <returns>True if the username meets the requirements, otherwise false.</returns>
    public static bool BeValidateUsername(string arg) =>
        new Regex("^(?=.{3,20}$)(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$").IsMatch(arg);

    /// <summary>
    /// Validates an email address to ensure it meets certain requirements.
    /// </summary>
    /// <param name="arg">The email address to validate.</param>
    /// <returns>True if the email address meets the requirements, otherwise false.</returns>
    public static bool BeValidateEmail(string arg) =>
        new Regex("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$").IsMatch(arg);

    /// <summary>
    /// Validates a phone number to ensure it is in a certain format.
    /// </summary>
    /// <param name="arg">The phone number to validate.</param>
    /// <returns>True if the phone number is in the correct format, otherwise false.</returns>
    public static bool BeValidatePhoneNumber(string arg) =>
        new Regex(
            "^(\\+994|0)?([ -])?(50|51|55|70|77|99)([ -])?(\\d{3})([ -])?(\\d{2})([ -])?(\\d{2})$"
        ).IsMatch(arg);

    /// <summary>
    /// Validates a credit card number to ensure it is in a certain format.
    /// </summary>
    /// <param name="arg">The credit card number to validate.</param>
    /// <returns>True if the credit card number is in the correct format, otherwise false.</returns>
    public static bool BeValidateCardNumber(string arg) =>
        new Regex(
            "^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|6(?:011|5[0-9]{2})[0-9]{12}|9[0-9]{15})$"
        ).IsMatch(arg);
}
