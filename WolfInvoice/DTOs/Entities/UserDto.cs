using WolfInvoice.Models.DataModels;

namespace WolfInvoice.DTOs.Entities;

/// <summary>
/// Represents a user data transfer object.
/// </summary>
public class UserDto
{
    /// <summary>
    /// </summary>
    public UserDto() { }

    /// <summary>
    /// Initialize instance
    /// </summary>
    /// <param name="user"></param>
    public UserDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        Address = user.Address;
        PhoneNumber = user.PhoneNumber;
        CreatedAt = user.CreatedAt;
    }

    /// <summary>
    /// Gets or sets the user's ID.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's address.
    /// </summary>
    public string? Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's phone number.
    /// </summary>
    public string? PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's creation date and time.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}
