using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class AuthorService : IAuthorService
{
    private ApplicationDbContext _dbContext;

    public AuthorService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Author> GetAuthor()
    {
        var books = _dbContext.Books.AsNoTracking();
        var maxTitleLength = await books.MaxAsync(b => b.Title.Length);
        
        var requiredAuthor = await books
            .Where(b => b.Title.Length == maxTitleLength)
            .Select(b => b.Author)
            .OrderByDescending(a => a.Id)
            .FirstOrDefaultAsync();

        return requiredAuthor;
    }

    public async Task<List<Author>> GetAuthors()
    {
        var authors = await _dbContext.Authors
            .AsNoTracking()
            .Select(a => new
            {
                Author = a,
                BooksCount = a.Books.Count(b => b.PublishDate.Year > 2015)
            })
            .Where(a => a.BooksCount % 2 == 0 && a.BooksCount != 0)
            .Select(a => a.Author)
            .ToListAsync();

        return authors;
    }
}