namespace food_delivery.Services;

using System.Text.Json;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using food_delivery.Models;
using food_delivery.Producers;

public class MerchantMenuService : IMerchantMenuService
{
    private readonly IMongoCollection<Merchant> _merchantsCollection;

    public MerchantMenuService(
        IOptions<FoodDeliveryDatabaseSettings> foodDeliveryDatabaseSettings
    )
    {
        var mongoClient = new MongoClient(foodDeliveryDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(foodDeliveryDatabaseSettings.Value.DatabaseName);
        _merchantsCollection = mongoDatabase.GetCollection<Merchant>(foodDeliveryDatabaseSettings.Value.MerchantsCollectionName);
    }

    /// <summary>
    /// Returns all merchant dishes.
    /// </summary>
    /// <param name="merchantId"></param>
    /// <returns></returns>
    public async Task<List<Dish>> GetAsync(string merchantId)
    {
        var merchant = await _merchantsCollection.Find(x => x.Id == merchantId).FirstOrDefaultAsync();
        return merchant.Dishes.ToList();
    }

    /// <summary>
    /// Returns informations about a dish.
    /// </summary>
    /// <param name="merchantId"></param>
    /// <param name="dishId"></param>
    /// <returns></returns>
    public async Task<Dish> GetDishAsync(string merchantId, string dishId)
    {
        var merchant = await _merchantsCollection.Find(x => x.Id == merchantId).FirstOrDefaultAsync();
        return merchant.Dishes.FirstOrDefault(d => d.Id == dishId);
    }

    /// <summary>
    /// Create a new dish in the merchant dishes list.
    /// </summary>
    /// <param name="merchantId"></param>
    /// <param name="newDish"></param>
    /// <returns></returns>
    public async Task<Dish?> CreateDishAsync(string merchantId, Dish newDish)
    {
        newDish.Id = ObjectId.GenerateNewId().ToString();

        var filter = Builders<Merchant>.Filter.Eq(m => m.Id, merchantId);
        var update = Builders<Merchant>.Update.Push(m => m.Dishes, newDish);

        UpdateResult result = await _merchantsCollection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

        if (result.IsModifiedCountAvailable && result.ModifiedCount == 0)
        {
            // The dish was not inserted or updated
            return null;
        }

        return await GetDishAsync(merchantId, newDish.Id);
    }

    public async Task<bool> UpdateDishAsync(string merchantId, Dish updatedDish)
    {
        try
        {
            var filter = Builders<Merchant>.Filter.And(
                Builders<Merchant>.Filter.Eq(m => m.Id, merchantId),
                Builders<Merchant>.Filter.ElemMatch(m => m.Dishes, d => d.Id == updatedDish.Id)
            );

            // var update = Builders<Merchant>.Update.Set(m => m.Dishes[-1], updatedDish);
            var update = Builders<Merchant>.Update.Set("Dishes.$", updatedDish);

            UpdateResult result = await _merchantsCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            // Handle any exceptions or log the error
            return false;
        }
    }


    public async Task<bool> DeleteDishAsync(string merchantId, string dishId)
    {
        try
        {
            var filter = Builders<Merchant>.Filter.And(
                Builders<Merchant>.Filter.Eq(m => m.Id, merchantId),
                Builders<Merchant>.Filter.ElemMatch(m => m.Dishes, d => d.Id == dishId)
            );

            var update = Builders<Merchant>.Update.PullFilter(m => m.Dishes, d => d.Id == dishId);

            UpdateResult result = await _merchantsCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }
        catch (Exception)
        {
            // Handle any exceptions or log the error
            return false;

        }
    }
}