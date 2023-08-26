

using LifeSaverBookShop.Controllers;
using LifeSaverBookShop.Models;
using Microsoft.AspNetCore.SignalR;

namespace LifeSaverBookShop.Hubs
{
    public class BooksHub: Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("GetConnectionId", this.Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            BookController.BooksDB.Remove(this.Context.ConnectionId);
            Console.WriteLine("DisconnectID:" + this.Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ClearProduct(Books book)
        {
            await Clients.All.SendAsync("ChangeBook", book);
        }
    }
}
