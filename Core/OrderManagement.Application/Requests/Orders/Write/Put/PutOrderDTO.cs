using AutoMapper;
using OrderManagement.Application.Common.Mappings;

namespace OrderManagement.Application.Requests.Orders.Write.Put
{
    public class PutOrderDTO : IMapWith<PutOrderRequest>
    {
        public string CustomerName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PutOrderDTO, PutOrderRequest>();
        }
    }
}
