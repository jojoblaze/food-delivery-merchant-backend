
using food_delivery.Models;
using Stripe;

namespace food_delivery.Services;

public class StripeProductsService
{
    public Product CreateProduct(string merchantId, Dish dish)
    {
        var options = new ProductCreateOptions
        {
            Id = $"product_{merchantId}_{dish.Id}",
            Name = dish.Name,
            Description = dish.Summary,
            DefaultPriceData = new ProductDefaultPriceDataOptions()
            {
                Currency = "eur",
                UnitAmountDecimal = dish.Price * 100 // in UnitAmountDecimal price is expressed in cents
            }
        };
        var service = new ProductService();
        Product newStripeProduct = service.Create(options);

        return newStripeProduct;
    }
}