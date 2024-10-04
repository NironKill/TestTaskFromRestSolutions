using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Enums;
using OrderManagement.Application.Requests.Orders.Read.GetAll;
using OrderManagement.Application.Requests.Orders.Read.GetById;
using OrderManagement.Application.Requests.Orders.Read.GetByStatus;
using OrderManagement.Application.Requests.Orders.Write.Create;
using OrderManagement.Application.Requests.Orders.Write.Delete;
using OrderManagement.Application.Requests.Orders.Write.Patch;
using OrderManagement.Application.Requests.Orders.Write.Put;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrderController : BaseController
    {
        private readonly IMapper _mapper;

        public OrderController(IMapper mapper) => _mapper = mapper;

        /// <summary>
        /// Creates the order
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /order/Create
        /// {
        /// 
        ///     customerName: "customer name",
        ///     totalAmount: "order amount"
        ///     currency: "order currency (USD, EUR, BYN, RUB, PLN)"
        /// }
        /// </remarks>
        /// <param name="dto">CreateOrderDTO object</param>
        /// <returns>Returns CreateOrderResponse</returns>
        /// <response code="201">Success</response>
        /// <response code="400">The server could not understand the request due to incorrect syntax</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody]CreateOrderDTO dto)
        {
            CreateOrderRequest request = _mapper.Map<CreateOrderRequest>(dto);

            CreateOrderResponse response = await Mediator.Send(request);
            return Created("", response);
        }

        /// <summary>
        /// Customer name update
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT /order/Put/bd36fda0-37f4-4dd4-8cb0-c223847ef691
        /// {
        /// 
        ///     customerName: "customer name",
        /// }
        /// </remarks>
        /// <param name="dto">PutOrderDTO object</param>
        /// <param name="id">Order id (guid)</param>
        /// <returns>Returns PutOrderResponse</returns>
        /// <response code="200">Success</response>
        /// <response code="400">The server could not understand the request due to incorrect syntax</response>
        /// <response code="404">The server can not find the requested resource.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromBody]PutOrderDTO dto, Guid id)
        {
            PutOrderRequest request = _mapper.Map<PutOrderRequest>(dto);
            request.Id = id;

            PutOrderResponse response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Change order status to cancelled
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PATCH /order/Patch/bd36fda0-37f4-4dd4-8cb0-c223847ef691
        /// </remarks>
        /// <param name="id">Order id (guid)</param>
        /// <returns>Returns PatchOrderResponse</returns>
        /// <response code="200">Success</response>
        /// <response code="400">The server could not understand the request due to incorrect syntax</response>
        /// <response code="404">The server can not find the requested resource.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(Guid id)
        {
            PatchOrderRequest request = new PatchOrderRequest
            {
                Id = id
            };

            PatchOrderResponse response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Delete the order
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE /order/Delete/bd36fda0-37f4-4dd4-8cb0-c223847ef691
        /// </remarks>
        /// <param name="id">Order id (guid)</param>
        /// <returns>Returns DeleteOrderResponse</returns>
        /// <response code="200">Success</response>
        /// <response code="400">The server could not understand the request due to incorrect syntax</response>
        /// <response code="404">The server can not find the requested resource.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            DeleteOrderRequest request = new DeleteOrderRequest
            {
                Id = id,
            };
            DeleteOrderResponse response = await Mediator.Send(request);

            return Ok(response);
        }

        /// <summary>
        /// Gets a list of all orders
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /order/GetAll
        /// </remarks>
        /// <returns>Returns GetAllOrderResponse</returns>
        /// <response code="200">Success</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            GetAllOrderRequest request = new GetAllOrderRequest();

            List<GetAllOrderResponse> responce = await Mediator.Send(request);
            return Ok(responce);
        }

        /// <summary>
        /// Gets the order by ID
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /order/GetById/bd36fda0-37f4-4dd4-8cb0-c223847ef691
        /// </remarks>
        /// <param name="id">Order id (guid)</param>
        /// <returns>Returns GetByIdOrderResponse</returns>
        /// <response code="200">Success</response>
        /// <response code="400">The server could not understand the request due to incorrect syntax</response>
        /// <response code="404">The server can not find the requested resource.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            GetByIdOrderRequest request = new GetByIdOrderRequest
            {
                Id = id,
            };
            GetByIdOrderResponse responce = await Mediator.Send(request);
            return Ok(responce);
        }

        /// <summary>
        /// Get a list of orders by status
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /order/GetByStatus?status=Pending
        /// </remarks>
        /// <param name="status">Order status (enum)</param>
        /// <returns>Returns GetByStatusOrderResponse</returns>
        /// <response code="200">Success</response>
        /// <response code="400">The server could not understand the request due to incorrect syntax</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByStatus([FromQuery]Status status)
        {
            GetByStatusOrderRequest request = new GetByStatusOrderRequest
            {
                Status = (int)status
            };

            List<GetByStatusOrderResponse> response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}
