using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using WolfInvoice.Interfaces.Documents;
using WolfInvoice.Models.DocumentModels;

namespace WolfInvoice.Documents.Components;

/// <summary>
/// Represents a component for displaying a payment address.
/// </summary>
public class AddressComponent : IComponent
{
    /// <summary>
    /// Gets the title of the address component.
    /// </summary>
    private string Title { get; }

    /// <summary>
    /// Gets the payment address.
    /// </summary>
    private PaymentAddress Address { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddressComponent"/> class with the specified title and address.
    /// </summary>
    /// <param name="title">The title of the address component.</param>
    /// <param name="address">The payment address to display.</param>
    public AddressComponent(string title, IPaymentAddress address)
    {
        Title = title;
        Address = address.GetPaymentAddress();
    }

    /// <summary>
    /// Composes the address component by adding it to the specified container.
    /// </summary>
    /// <param name="container">The container to add the address component to.</param>
    public void Compose(IContainer container)
    {
        container
            .ShowEntire()
            .Column(column =>
            {
                column.Spacing(2);

                column.Item().Text(Title).SemiBold();
                column.Item().PaddingBottom(5).LineHorizontal(1);

                column.Item().Text(Address.Name);
                column.Item().Text(Address.Address);
                column.Item().Text(Address.PhoneNumber);
                column.Item().Text(Address.Email);
            });
    }
}
