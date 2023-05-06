using Bogus;
using WolfInvoice.Enums;
using WolfInvoice.Models.DataModels;

namespace WolfInvoice.Services;

/// <summary>
/// A service that generates fake data for testing purposes.
/// </summary>
public class FakerService
{
    /// <summary>
    /// Generates a set of fake customers.
    /// </summary>
    /// <param name="count">The number of customers to generate.</param>
    /// <returns>A hashset of fake customers.</returns>
    public static HashSet<Customer> GenerateSeedDataForCustomer(int count) =>
        new Faker<Customer>()
            .RuleFor(c => c.Id, f => f.Random.Guid().ToString("N").ToLower())
            .RuleFor(c => c.Name, f => f.Name.FullName())
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.CreditCard, f => f.Finance.CreditCardNumber())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(c => c.CreatedAt, f => f.Date.Past(1))
            .RuleFor(c => c.EntityStatus, f => f.PickRandom<EntityStatus>())
            .RuleFor(
                c => c.UpdatedAt,
                f => f.Date.Between(DateTime.Now.AddMonths(-1), DateTime.Now)
            )
            .RuleFor(
                c => c.DeletedAt,
                (f, u) => u.EntityStatus == EntityStatus.Deleted ? f.Date.Past(1) : null
            )
            .Generate(count)
            .ToHashSet();

    /// <summary>
    /// Generates a set of fake invoice rows.
    /// </summary>
    /// <param name="count">The number of invoice rows to generate.</param>
    /// <returns>A HashSet of fake invoice rows.</returns>
    public static HashSet<InvoiceRow> GenerateSeedDataForInvoiceRow(int count) =>
        new Faker<InvoiceRow>()
            .RuleFor(i => i.Id, f => f.Random.Guid().ToString("N").ToLower())
            .RuleFor(i => i.Service, f => f.Commerce.ProductName())
            .RuleFor(
                i => i.Quantity,
                f => Math.Round(f.Random.Decimal(1, 10), 2, MidpointRounding.AwayFromZero)
            )
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(i => i.Sum, (f, i) => i.Quantity * i.Amount)
            .Generate(count)
            .ToHashSet();

    /// <summary>
    /// Generates a set of fake invoices.
    /// </summary>
    /// <param name="count">The number of invoices to generate.</param>
    /// <returns>A HashSet of fake invoices.</returns>
    public static HashSet<Invoice> GenerateSeedDataForInvoice(int count) =>
        new Faker<Invoice>()
            .RuleFor(i => i.Id, f => f.Random.Guid().ToString("N").ToLower()[..8])
            .RuleFor(i => i.StartDate, f => f.Date.Past(1))
            .RuleFor(i => i.EndDate, (f, u) => u.StartDate.AddDays(f.Random.Int(1, 30)))
            .RuleFor(
                i => i.TotalSum,
                f => Math.Round(f.Random.Decimal(1, 10000), 2, MidpointRounding.AwayFromZero)
            )
            .RuleFor(
                i => i.Discount,
                f =>
                    f.Random.Bool(0.5f)
                        ? Math.Round(
                            f.Random.Decimal(0.01M, 0.2M),
                            2,
                            MidpointRounding.AwayFromZero
                        )
                        : (decimal?)null
            )
            .RuleFor(i => i.Comment, f => f.Random.Bool(0.5f) ? f.Lorem.Sentence() : null)
            .RuleFor(i => i.Status, f => f.PickRandom<InvoiceStatus>())
            .RuleFor(i => i.EntityStatus, f => f.PickRandom<EntityStatus>())
            .RuleFor(i => i.CreatedAt, f => f.Date.Past(2))
            .RuleFor(i => i.UpdatedAt, (f, u) => u.CreatedAt.AddDays(f.Random.Int(1, 7)))
            .RuleFor(
                i => i.DeletedAt,
                (f, u) => u.EntityStatus == EntityStatus.Deleted ? f.Date.Past(1) : null
            )
            .Generate(count)
            .ToHashSet();

    /// <summary>
    /// Generates a set of fake invoices filled. Rows count: 5 - 40 .
    /// </summary>
    /// <param name="count">The number of invoices to generate.</param>
    /// <returns>A HashSet of fake invoices.</returns>
    public static HashSet<Invoice> GenerateSeedDataForInvoiceFilled(int count) =>
        new Faker<Invoice>()
            .RuleFor(i => i.Id, f => f.Random.Guid().ToString("N").ToLower()[..8])
            .RuleFor(i => i.StartDate, f => f.Date.Past(1))
            .RuleFor(i => i.EndDate, (f, u) => u.StartDate.AddDays(f.Random.Int(1, 30)))
            .RuleFor(
                i => i.TotalSum,
                f => Math.Round(f.Random.Decimal(1, 10000), 2, MidpointRounding.AwayFromZero)
            )
            .RuleFor(
                i => i.Discount,
                f =>
                    f.Random.Bool(0.5f)
                        ? Math.Round(
                            f.Random.Decimal(0.01M, 0.2M),
                            2,
                            MidpointRounding.AwayFromZero
                        )
                        : null
            )
            .RuleFor(i => i.Comment, f => f.Random.Bool(0.5f) ? f.Lorem.Sentence() : null)
            .RuleFor(i => i.Status, f => f.PickRandom<InvoiceStatus>())
            .RuleFor(i => i.EntityStatus, f => f.PickRandom<EntityStatus>())
            .RuleFor(i => i.CreatedAt, f => f.Date.Past(2))
            .RuleFor(i => i.UpdatedAt, (f, u) => u.CreatedAt.AddDays(f.Random.Int(1, 7)))
            .RuleFor(i => i.Rows, f => GenerateSeedDataForInvoiceRow(f.Random.Int(5, 40)))
            .RuleFor(
                i => i.DeletedAt,
                (f, u) => u.EntityStatus == EntityStatus.Deleted ? f.Date.Past(1) : null
            )
            .Generate(count)
            .ToHashSet();

    /// <summary>
    /// Generates a set of fake users.
    /// </summary>
    /// <param name="count">The number of users to generate.</param>
    /// <param name="cryptService">For hashing user's password</param>
    /// <returns>A HashSet of fake users.</returns>
    public static HashSet<User> GenerateSeedDataForUser(int count, CryptService cryptService) =>
        new Faker<User>()
            .RuleFor(u => u.Id, f => f.Random.Guid().ToString("N").ToLower())
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Address, f => f.Address.FullAddress())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => cryptService.CryptPassword(f.Internet.Password()))
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.CreatedAt, f => f.Date.PastOffset())
            .RuleFor(u => u.UpdatedAt, f => f.Date.RecentOffset())
            .RuleFor(u => u.EntityStatus, EntityStatus.Active)
            .Generate(count)
            .ToHashSet();
}
