using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        public int OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public int Currency { get; set; }

        /// <summary>
        /// Order status: Pending, Processing, Completed, Cancelled
        /// </summary>
        public int Status { get; set; }

        public int Priority { get; set; }

        /// <summary>
        /// The base currency is USD
        /// </summary>
        public decimal TotalAmountInBaseCurrency { get; set; }
    }
}
