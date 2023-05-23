using Microsoft.AspNetCore.Mvc;

namespace food_delivery.Controllers;

[ApiController]
[Route("[controller]")]
public class ShopController : ControllerBase
{
    private static readonly List<Dish> Dishes = new List<Dish>
    {
        new Dish(){ Name = "Patatine Fritte", Price = 3.5m }, 
        new Dish(){ Name = "Olive Ascolana", Price = 3.5m }, 
        new Dish(){ Name = "Pasta ragù", Price = 5m }, 
        new Dish(){ Name = "Pasta carbonara", Price = 5m }, 
        new Dish(){ Name = "Pasto aglio e olio", Price = 4m }, 
        new Dish(){ Name = "Bistecca di pollo", Price = 6m }, 
        new Dish(){ Name = "Bistecca di manzo", Price = 7m }, 
        new Dish(){ Name = "Merluzzo", Price = 8m }, 
        new Dish(){ Name = "Insalata", Price = 2m }, 
        new Dish(){ Name = "Spinaci", Price = 2.5m }
    };

    private readonly ILogger<ShopController> _logger;

    public ShopController(ILogger<ShopController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "dishes")]
    public IEnumerable<Dish> Get()
    {
        return Dishes.ToArray();
    }

    [HttpPost(Name = "dishes")]
    public async Task<ActionResult<Dish>> Post([FromBody] Dish dish)
    {
        Dishes.Add(dish);
        return CreatedAtAction(nameof(Get), new { id = 1 }, dish);
    }
}
