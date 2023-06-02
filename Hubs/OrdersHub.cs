using System.Threading.Tasks;
using food_delivery.Models;
using Microsoft.AspNetCore.SignalR;

namespace food_delivery.Hubs
{
    public interface IOrdersHubClient
    {
        Task TakeOrder(Order message);
    }

    public class OrdersHub : Hub<IOrdersHubClient>
    {
        public async Task SendOrderToClients(Order message)
        {
            await Clients.All.TakeOrder(message);
        }
    }
}