using WolfInvoice.Models.DocumentModels;

namespace WolfInvoice.Interfaces.Documents;

/// <summary>
/// Interface to define payment address behavior
/// </summary>
public interface IPaymentAddress
{
    /// <summary>
    /// Get the payment address object
    /// </summary>
    /// <returns>The payment address object</returns>
    public PaymentAddress GetPaymentAddress();
}
