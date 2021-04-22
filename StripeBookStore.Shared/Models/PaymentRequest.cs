using System.ComponentModel.DataAnnotations;

namespace StripeBookStore.Shared.Models
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
        [Required]
        public string Sku { get; }

        /// <summary>
        /// Customer for Product
        /// </summary>
        /// <example>cus_testId</example>
        [Required]
        public string Customer { get; }
    }
}
