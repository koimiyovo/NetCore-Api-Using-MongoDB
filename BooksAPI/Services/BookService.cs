using BooksAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksAPI.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }

        public async Task<List<Book>> Get() => await _books.Find(book => true).ToListAsync();

        public async Task<Book> Get(string id) => await _books.Find(b => b.Id == id).FirstOrDefaultAsync();

        public async Task<Book> Create(Book book)
        {
            await _books.InsertOneAsync(book);
            return book;
        }

        public async Task Update(string id, Book bookIn) => await _books.ReplaceOneAsync(b => b.Id == id, bookIn);

        public async Task Remove(Book bookIn) => await _books.DeleteOneAsync(b => b.Id == bookIn.Id);
        
        public async Task Remove(string id) => await _books.DeleteOneAsync(b => b.Id == id);
    }
}
