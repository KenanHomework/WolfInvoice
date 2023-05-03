using WolfInvoice.DTOs.Entities;
using WolfInvoice.DTOs.Requests.User;
using WolfInvoice.Exceptions.EntityExceptions;
using WolfInvoice.Models.DataModels;

namespace WolfInvoice.Interfaces.EntityServices;

/// <summary>
/// Interface for <see cref="User"/> service operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets a user by the given id.
    /// </summary>
    /// <param name="id">The user id.</param>
    /// <returns>The <see cref="User"/> object, or <see langword="null"/> if the user was not found.</returns>
    public Task<UserDto?> GetUserById(string id);

    /// <summary>
    /// Adds a new <see cref="User"/>
    /// </summary>
    /// <param name="user">The <see cref="User"/> to add</param>
    /// <returns><see langword="true"/> if the <see cref="User"/> was added successfully, <see langword="false"/> otherwise</returns>
    /// <exception cref="EntityConflictException"/>
    public Task<bool> AddUser(User user);

    /// <summary>
    /// Creates a new user with the given id and request data.
    /// </summary>
    /// <param name="request">The user data to create.</param>
    /// <returns>The newly created user.</returns>
    /// <exception cref="EntityConflictException"/>
    public Task<UserDto> CreateUser(CreateUserRequest request);

    /// <summary>
    /// Updates an existing user with the given id and request data.
    /// </summary>
    /// <param name="id">The user id.</param>
    /// <param name="request">The user data to update.</param>
    /// <returns>The updated user.</returns>
    /// <exception cref="EntityNotFoundException"/>
    /// <exception cref="EntityConflictException"/>
    public Task<UserDto> EditUser(string id, UpdateUserRequest request);

    /// <summary>
    /// Changes the password of the user with the given id.
    /// </summary>
    /// <param name="id">The user id.</param>
    /// <param name="request"></param>
    /// <returns>The updated user object.</returns>
    /// <exception cref="EntityNotFoundException"/>
    /// <exception cref="EntityPasswordInvalidException"/>
    public Task<UserDto> ChangeUserPassword(string id, ChangePasswordUserRequest request);

    /// <summary>
    /// Deletes the user with the given id.
    /// </summary>
    /// <param name="id">The user id.</param>
    /// <returns><see langword="true"/> if the user was deleted, <see langword="false"/> otherwise.</returns>
    /// <exception cref="EntityNotFoundException"/>
    public Task<bool> DeleteUser(string id);

    /// <summary>
    /// Checks if a <see cref="User"/> with the given ID exists
    /// </summary>
    /// <param name="id">The ID of the <see cref="User"/> to check</param>
    /// <returns><see langword="true"/> if the <see cref="User"/> exists, <see langword="false"/> otherwise</returns>
    public Task<bool> UserExistsById(string id);

    /// <summary>
    /// Checks if a <see cref="User"/> with the given Email address exists
    /// </summary>
    /// <param name="email">The ID of the <see cref="User"/> to check</param>
    /// <returns><see langword="true"/> if the <see cref="User"/> exists, <see langword="false"/> otherwise</returns>
    public Task<bool> UserExistsByEmail(string email);

    /// <summary>
    /// Converts a <see cref="User"/> entity to a <see cref="UserDto"/> data transfer object.
    /// </summary>
    /// <param name="user">The <see cref="User"/> entity to convert.</param>
    /// <returns> The converted <see cref="UserDto"/> object, or null if the input is null.</returns>
    public UserDto? ConvertToDto(User user);
}
