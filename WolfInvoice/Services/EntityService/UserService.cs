using Microsoft.EntityFrameworkCore;
using WolfInvoice.Data;
using WolfInvoice.DTOs.Entities;
using WolfInvoice.DTOs.Requests.User;
using WolfInvoice.Exceptions.EntityExceptions;
using WolfInvoice.Interfaces.EntityServices;
using WolfInvoice.Models.DataModels;

namespace WolfInvoice.Services.EntityService;

/// <summary>
/// Service for <see cref="User"/> service operations.
/// </summary>

public class UserService : IUserService
{
    private readonly WolfInvoiceContext _context;
    private readonly CryptService _cryptService;

    /// <summary>
    /// Initialize object
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cryptService"></param>
    public UserService(WolfInvoiceContext context, CryptService cryptService)
    {
        _context = context;
        _cryptService = cryptService;
    }

    /// <inheritdoc/>
    public async Task<bool> AddUser(User user)
    {
        if (await UserExistsByEmail(user.Email))
            throw new EntityConflictException("There is already a user with this email!");

        user.Id = IdGeneratorService.GetUniqueId();

        user.Email = user.Email.ToLower();
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        LogService.LogUserAction(user.Id, Enums.UserActions.Added);

        return true;
    }

    /// <inheritdoc/>
    public async Task<UserDto> ChangeUserPassword(string id, ChangePasswordUserRequest request)
    {
        var user =
            await _context.Users.FirstOrDefaultAsync(
                u => u.Id.Equals(id) && u.EntityStatus == Enums.EntityStatus.Active
            ) ?? throw new EntityNotFoundException("User not found of given id!");

        if (!_cryptService.CheckPassword(request.CurrentPassword, user.Password))
            throw new EntityPasswordInvalidException("User password is invalid!");

        if (request.CurrentPassword.Equals(request.NewPassword))
            throw new EntityPasswordInvalidException(
                "The user's new password cannot be the same as the old one!"
            );

        user.Password = _cryptService.CryptPassword(request.NewPassword);
        user.UpdatedAt = DateTimeOffset.Now;

        LogService.LogUserAction(id, Enums.UserActions.PasswordChanged);

        return new UserDto(user);
    }

    /// <inheritdoc/>
    public async Task<UserDto> CreateUser(CreateUserRequest request)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(request.Email));

        if (user is not null)
            throw new EntityConflictException("There is already a user with this email!");

        user = new()
        {
            Id = IdGeneratorService.GetUniqueId(),
            Name = request.Name,
            Address = request.Address,
            Email = request.Email.ToLower(),
            PhoneNumber = request.PhoneNumber,
            EntityStatus = Enums.EntityStatus.Active,
            CreatedAt = DateTimeOffset.Now,
            UpdatedAt = DateTimeOffset.Now
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        LogService.LogUserAction(user.Id, Enums.UserActions.Created);

        return new UserDto(user);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteUser(string id)
    {
        var user =
            await _context.Users.FirstOrDefaultAsync(
                u => u.Id.Equals(id) && u.EntityStatus == Enums.EntityStatus.Active
            ) ?? throw new EntityNotFoundException("User not found of given id!");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        LogService.LogUserAction(id, Enums.UserActions.Deleted);

        return true;
    }

    /// <inheritdoc/>
    public async Task<UserDto> EditUser(string id, UpdateUserRequest request)
    {
        var user =
            await _context.Users.FirstOrDefaultAsync(
                u => u.Id.Equals(id) && u.EntityStatus == Enums.EntityStatus.Active
            ) ?? throw new EntityNotFoundException("User not found of given id!");

        user.Name = request.Name ?? user.Name;
        user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
        user.Address = request.Address ?? user.Address;

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            if (await UserExistsByEmail(request.Email.ToLower()))
                throw new EntityConflictException("There is already a user with this email!");

            user.Email = request.Email.ToLower();
        }

        user.UpdatedAt = DateTime.UtcNow;
        _context.Update(user);
        await _context.SaveChangesAsync();

        LogService.LogUserAction(user.Id, Enums.UserActions.Edited);

        return new UserDto(user);
    }

    /// <inheritdoc/>
    public async Task<UserDto?> GetUserById(string id) =>
        ConvertToDto(
            await _context.Users.FirstOrDefaultAsync(
                u => u.Id.Equals(id) && u.EntityStatus == Enums.EntityStatus.Active
            )
        );

    /// <inheritdoc/>
    public async Task<bool> UserExistsById(string id) =>
        await _context.Users.AnyAsync(u => u.Id.Equals(id));

    /// <inheritdoc/>
    public async Task<bool> UserExistsByEmail(string email) =>
        await _context.Users.AnyAsync(u => u.Email.Equals(email));

    /// <inheritdoc/>
    public UserDto? ConvertToDto(User? user) => user is null ? null : new UserDto(user);
}
