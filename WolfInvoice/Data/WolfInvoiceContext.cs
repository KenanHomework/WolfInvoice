using Microsoft.EntityFrameworkCore;
using WolfInvoice.Models.DataModels;

namespace WolfInvoice.Data;

/// <summary>
/// Represents a database context for WolfMail.
/// </summary>
public class WolfInvoiceContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WolfInvoiceContext"/> class with the specified options.
    /// </summary>
    /// <param name="options">The options to be used by the context.</param>
    public WolfInvoiceContext(DbContextOptions<WolfInvoiceContext> options)
        : base(options) =>
        // Ensure that the database is created if it doesn't exist.
        Database.EnsureCreated();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Customer -> *User relation.
        modelBuilder.Entity<Customer>().HasOne(c => c.User).WithMany(u => u.Customers);

        // User -> *Invoice relation.
        modelBuilder.Entity<Invoice>().HasOne(i => i.User).WithMany(u => u.Invoices);

        // Invoice -> *Customer relation.
        modelBuilder.Entity<Invoice>().HasOne(i => i.Customer).WithMany(u => u.Invoices);

        // Invoice -> *InvoiceRow relation.
        modelBuilder.Entity<InvoiceRow>().HasOne(i => i.Invoice).WithMany(u => u.Rows);
    }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="User"/> entities in the context.
    /// </summary>
    /// <value>The <see cref="DbSet{TEntity}"/> for <see cref="User"/> entities in the context.</value>
    public DbSet<User> Users { get; set; } = default!;

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="User"/> entities in the context.
    /// </summary>
    /// <value>The <see cref="DbSet{TEntity}"/> for <see cref="User"/> entities in the context.</value>
    public DbSet<Customer> Customers { get; set; } = default!;

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="User"/> entities in the context.
    /// </summary>
    /// <value>The <see cref="DbSet{TEntity}"/> for <see cref="User"/> entities in the context.</value>
    public DbSet<Invoice> Invoices { get; set; } = default!;

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="User"/> entities in the context.
    /// </summary>
    /// <value>The <see cref="DbSet{TEntity}"/> for <see cref="User"/> entities in the context.</value>
    public DbSet<InvoiceRow> InvoiceRows { get; set; } = default!;
}
