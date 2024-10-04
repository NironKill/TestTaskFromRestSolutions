using System.Text.Json.Serialization;

namespace OrderManagement.Application.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        /// <summary>
        /// The order is pending processing
        /// </summary>
        Pending = 1,

        /// <summary>
        /// The order is being processed
        /// </summary>
        Processing = 2,

        /// <summary>
        /// Order successfully processed
        /// </summary>
        Completed = 3,

        /// <summary>
        /// The order has been cancelled and cannot be processed further
        /// </summary>
        Cancelled = 4
    }
}
