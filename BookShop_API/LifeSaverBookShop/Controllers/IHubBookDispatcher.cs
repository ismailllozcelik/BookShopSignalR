using LifeSaverBookShop.Models;

namespace LifeSaverBookShopAPI.Controllers
{
    public interface IHubBookDispatcher
    {
        Task ChangeBook(Books book);
    }
}
