namespace StripeBookStore.Shared.Models.DTOs
{
    public class PaymentRequest
    {
        public PaymentRequest(string sku, string customer)
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
