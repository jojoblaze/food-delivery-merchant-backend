using food_delivery.Services;
using food_delivery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace food_delivery.Controllers;

[ApiController]
[Route("api/merchants/{merchantId:length(24)}")]
public class MerchantMenuController : ControllerBase
{

    private readonly ILogger<MerchantMenuController> _logger;

    private readonly MerchantMenuService _merchantsMenuService;

    public MerchantMenuController(MerchantMenuService merchantsMenuService, ILogger<MerchantMenuController> logger)
    {
        _merchantsMenuService = merchantsMenuService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Dish>>> Get([FromRoute] string merchantId)
    {
        var dishes = await _merchantsMenuService.GetAsync(merchantId);

        if (dishes is null)
        {
            return NotFound();
        }

        return dishes;
    }

    [HttpGet("{dishId:length(24)}")]
    public async Task<ActionResult<Dish>> GetDish([FromRoute] string merchantId, string dishId)
    {
        var dish = await _merchantsMenuService.GetDishAsync(merchantId, dishId);

        if (dish is null)
        {
            return NotFound();
        }

        return dish;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromRoute] string merchantId, Dish newDish)
    {
        Dish? createdDish = await _merchantsMenuService.CreateDishAsync(merchantId, newDish);
        if (createdDish != null)
            return CreatedAtAction(nameof(GetDish), new { merchantId = merchantId, dishId = newDish.Id }, newDish);
        else
            return BadRequest("Failed to create the dish.");
    }

    [HttpPut("{dishId:length(24)}")]
    public async Task<IActionResult> UpdateDish([FromRoute] string merchantId, [FromRoute] string dishId, Dish updatedDish)
    {
        var dish = await _merchantsMenuService.GetDishAsync(merchantId, dishId);
        if (dish == null)
        {
            return NotFound();
        }

        // Update the dish properties
        dish.Name = updatedDish.Name;
        dish.Price = updatedDish.Price;
        dish.Summary = updatedDish.Summary;

        bool isSuccess = await _merchantsMenuService.UpdateDishAsync(merchantId, dish);
        if (!isSuccess)
        {
            // Handle the update failure
            return BadRequest("Failed to update the dish.");
        }

        return NoContent(); // Return 204 No Content to indicate a successful update
    }


    [HttpDelete("{dishId:length(24)}")]
    public async Task<IActionResult> DeleteDish([FromRoute] string merchantId, [FromRoute] string dishId)
    {
        var dish = await _merchantsMenuService.GetDishAsync(merchantId, dishId);
        if (dish == null)
        {
            return NotFound();
        }

        bool isSuccess = await _merchantsMenuService.DeleteDishAsync(merchantId, dishId);
        if (!isSuccess)
        {
            // Handle the deletion failure
            return BadRequest("Failed to delete the dish.");
        }

        return NoContent(); // Return 204 No Content to indicate a successful deletion
    }

}
