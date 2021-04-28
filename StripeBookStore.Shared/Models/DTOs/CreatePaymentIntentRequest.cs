namespace StripeBookStore.Shared.Models.DTOs
{
    public class CreatePaymentIntentRequest
    {
        public CreatePaymentIntentRequest(string sku, string customer = null)
        {
            Sku = sku;
            Customer = customer;
        }
        /// <summary>
        /// Product SKU
        /// </summary>
        /// <example>prod_testId</example>
        public string Sku { get; }

        /// <summary>
        /// Customer for Product
        /// </summary>
        /// <example>cus_testId</example>
        public string Customer { get; }
    }
}
