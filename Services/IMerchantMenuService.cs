namespace food_delivery.Services;

using food_delivery.Models;

public interface IMerchantMenuService
{
    Task<Dish?> CreateDishAsync(string merchantId, Dish newDish);
    Task<bool> DeleteDishAsync(string merchantId, string dishId);
    Task<List<Dish>> GetAsync(string merchantId);
    Task<Dish> GetDishAsync(string merchantId, string dishId);
    Task<bool> UpdateDishAsync(string merchantId, Dish updatedDish);
}