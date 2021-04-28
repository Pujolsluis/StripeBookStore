using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using StripeBookStore.Shared.Interfaces;
using StripeBookStore.Shared.Models;

namespace StripeBookStore.API.Hubs
{
    public class PaymentsHub : Hub<IPaymentsHub>
    {
        public async Task SendPaymentEvent(PaymentEvent paymentEvent)
        {
            await Clients.All.SendPaymentEvent(paymentEvent);
        }
    }
}
