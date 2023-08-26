using LifeSaverBookShop.Models;
using LifeSaverBookShopAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeSaverBookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        // Herbir client’ın saklanacağı, static dictionary listesi.
        public static Dictionary<string, List<Books>> BooksDB = new();

        //Herbir client’a gönderilen 5’li kitap listesi.
        public static List<Books> Books = new();

        // Test amaçlı DB rolunde olan, kitap havuzunun bulunduğu string dizi. Herbir client için rastgele seçilen beş kitap, bu liste içinden alınmaktadır.
        private static readonly string[] BookList = new[]
        {
            "Atomik_Alışkanlıklar", "Çantamdan_Fil_Çıktı", "Uzaya_Giden_Tren", "Kayıp_Ağaçlar_Adası", "İrade_Terbiyesi", "Var_mısın", "Geliştiren_Anne-Baba", "Evlenmeden_Önce"
        };

        IHubBookDispatcher _bookDispatcher;

        public BookController(IHubBookDispatcher bookDispatcher)
        {
            _bookDispatcher = bookDispatcher;
        }

        [HttpGet("{connectionId}")]
        public List<Books> Get(string connectionId)
        {
            var random = new Random();

            Books = Enumerable.Range(1, 10).Select(index => new Models.Books
            {
                Id = index,
                Name = BookList[random.Next(BookList.Length)],
                Price = random.Next(100) + 30,
                CreatedDate = DateTime.Now,

            }).GroupBy(item => item.Name)
                .Select(grp => grp.First()).Take(5)
                .ToList();

            Books.ForEach(b => b.ImgPath = b.Name + ".jpg");
            BooksDB.Add(connectionId, Books);
            return Books;
        }

        [HttpPost("UpdateBook")]
        public async Task UpdateBook([FromBody] Books book)
        {
            bool isChange = false;
            foreach (var booksList in BooksDB)
            {
                var updateBook = booksList.Value.Where(b => b.Name == book.Name).FirstOrDefault();
                if (updateBook != null)
                {
                    isChange = true;
                    booksList.Value.Remove(updateBook);
                    booksList.Value.Add(book);
                    BooksDB[booksList.Key] = booksList.Value;
                }

                if (isChange)
                {
                    await this._bookDispatcher.ChangeBook(book);
                }
            }
        }
    }
}
