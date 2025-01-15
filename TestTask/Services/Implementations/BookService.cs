using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class BookService : IBookService
{
    private ApplicationDbContext _dbContext;

    public BookService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Book> GetBook()
    {
        var books = _dbContext.Books
            .AsNoTracking();

        var bookWithMaxProfit = await books
            .Select(b => new
            {
                Book = b,
                Profit = b.Price * b.QuantityPublished
            })
            .OrderByDescending(b => b.Profit)
            .FirstOrDefaultAsync();

        return bookWithMaxProfit?.Book;
    }

    public async Task<List<Book>> GetBooks()
    {
        var books = _dbContext.Books
            .AsNoTracking();

        var requiredBooks = await books
            .Where(b => b.Title.Contains("Red") && b.PublishDate > new DateTime(2012, 5, 25))
            .ToListAsync();

        return requiredBooks;
    }
}