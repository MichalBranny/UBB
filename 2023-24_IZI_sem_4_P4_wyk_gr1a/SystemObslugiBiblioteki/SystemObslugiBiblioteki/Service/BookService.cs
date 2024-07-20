using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using SystemObslugiBiblioteki.Enums;
using SystemObslugiBiblioteki.Model;
using SystemObslugiBiblioteki.Persistence;

namespace SystemObslugiBiblioteki.Service
{
    public class BookService
    {
        private readonly IAppDbContext _appDbContext;

        public BookService(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Books> GetBooks(int category = 99)
        {
            if (category == 99)
            {
                return _appDbContext.Books;
            }
            else
            {
                if (!Enum.IsDefined(typeof(BookCategory), category))
                {
                    throw new ArgumentException("Invalid category value", nameof(category));
                }

                return _appDbContext.Books.Where(w => w.BookCategory == (BookCategory)category);
            }
        }

        public async Task<Books> CreateBook(string title, string author, int category, bool isAvailable)
        {
            if (!Enum.IsDefined(typeof(BookCategory), category))
            {
                throw new ArgumentException("Invalid category value", nameof(category));
            }

            var book = new Books()
            {
                Title = title,
                Author = author,
                BookCategory = (BookCategory)category,
                IsAvailable = isAvailable
            };

            _appDbContext.Books.AddOrUpdate(book);

            var result = _appDbContext.Books.Add(book);
            await _appDbContext.SaveChangesAsync();
            return result;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var bookToRemove = _appDbContext.Books.FirstOrDefault(b => b.Id == id);
            if (bookToRemove == null)
            {
                return false;
            }

            var result = _appDbContext.Books.Remove(bookToRemove);
            await _appDbContext.SaveChangesAsync();

            return result != null;
        }

        public async Task SetBookAvailable(int id, bool isAvailable)
        {
            var bookToChangeAvailable = _appDbContext.Books.FirstOrDefault(b => b.Id == id);
            if (bookToChangeAvailable != null)
            {
                bookToChangeAvailable.IsAvailable = isAvailable;
                _appDbContext.Books.AddOrUpdate(bookToChangeAvailable);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<Books> ModifyBook(int id, string title, string author, BookCategory category, bool isAvailable)
        {
            var book = _appDbContext.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return null;
            }

            book.Title = title;
            book.Author = author;
            book.BookCategory = category;
            book.IsAvailable = isAvailable;

            _appDbContext.Books.AddOrUpdate(book);
            await _appDbContext.SaveChangesAsync();

            return book;
        }
    }
}
