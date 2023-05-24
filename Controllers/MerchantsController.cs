using food_delivery.Models;
using food_delivery.Services;
using Microsoft.AspNetCore.Mvc;

namespace food_delivery.Controllers;

[ApiController]
[Route("[controller]")]
public class MerchantsController : ControllerBase
{

    private readonly ILogger<MerchantsController> _logger;

    private readonly MerchantsService _merchantsService;

    public MerchantsController(MerchantsService merchantsService, ILogger<MerchantsController> logger)
    {
        _merchantsService = merchantsService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<List<Merchant>> Get()
    {
        return await _merchantsService.GetAsync();
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Merchant>> Get(string id)
    {
        var merchant = await _merchantsService.GetAsync(id);

        if (merchant is null)
        {
            return NotFound();
        }

        return merchant;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Merchant newMerchant)
    {
        await _merchantsService.CreateAsync(newMerchant);

        return CreatedAtAction(nameof(Get), new { id = newMerchant.Id }, newMerchant);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Merchant updatedMerchant)
    {
        var book = await _merchantsService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        updatedMerchant.Id = book.Id;

        await _merchantsService.UpdateAsync(id, updatedMerchant);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await _merchantsService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        await _merchantsService.RemoveAsync(id);

        return NoContent();
    }
}
