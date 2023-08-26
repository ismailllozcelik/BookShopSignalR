using LifeSaverBookShop.Hubs;
using LifeSaverBookShop.Models;
using Microsoft.AspNetCore.SignalR;

namespace LifeSaverBookShopAPI.Controllers
{
    public class HubBookDispatcher : IHubBookDispatcher
    {
        private readonly IHubContext<BooksHub> _hubContext;

        public HubBookDispatcher(IHubContext<BooksHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task ChangeBook(Books book)
        {
            await this._hubContext.Clients.All.SendAsync("ChangeBook", book);
        }
    }
}
