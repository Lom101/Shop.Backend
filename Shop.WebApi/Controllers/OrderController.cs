using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Order.Requests;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET: api/order
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }
    
    // GET: api/order/get_by_user_id
    [HttpGet("get_by_user_id")]
    public async Task<IActionResult> GetOrdersByUserId(string userId)
    {
        var orders = await _orderService.GetOrdersByUserId(userId);
        return Ok(orders);
    }


    // GET: api/order/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    // POST: api/order
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest createOrderRequest)
    {
        var order = await _orderService.AddOrderAsync(createOrderRequest);
        if (order == null)
            return BadRequest("Не удалось создать заказ.");

        return Ok(order);
    }


    // PUT: api/order/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderRequest updateOrderRequest)
    {
        if (id != updateOrderRequest.Id)
        {
            return BadRequest("order ID mismatch");
        }

        var isUpdated = await _orderService.UpdateOrderAsync(updateOrderRequest);
        if (!isUpdated)
        {
            return NotFound();
        }
        return NoContent();
    }

    // DELETE: api/order/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var isDeleted = await _orderService.DeleteOrderAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}