using AutoMapper;
using OrderManagement.Application.Common.Mappings;

namespace OrderManagement.Application.Requests.Orders.Write.Create
{
    public class CreateOrderDTO : IMapWith<CreateOrderRequest>
    {
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateOrderDTO, CreateOrderRequest>();
        }
    }
}
