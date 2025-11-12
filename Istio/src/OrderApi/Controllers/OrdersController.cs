using Microsoft.AspNetCore.Mvc;

namespace OrderApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private static readonly List<Order> Orders = new()
    {
        new Order { Id = 1, ProductId = 1, Quantity = 1, Total = 999.99m },
        new Order { Id = 2, ProductId = 2, Quantity = 2, Total = 59.98m }
    };

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(Orders);
    }

    [HttpPost]
    public IActionResult Create(Order order)
    {
        order.Id = Orders.Count + 1;
        Orders.Add(order);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var order = Orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return NotFound();
        return Ok(order);
    }
}

public class Order
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
}