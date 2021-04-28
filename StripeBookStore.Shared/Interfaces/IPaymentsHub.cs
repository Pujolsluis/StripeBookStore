using System;
using System.Threading.Tasks;
using StripeBookStore.Shared.Models;

namespace StripeBookStore.Shared.Interfaces
{
    public interface IPaymentsHub
    {
        Task SendPaymentEvent(PaymentEvent paymentEvent);
    }
}
