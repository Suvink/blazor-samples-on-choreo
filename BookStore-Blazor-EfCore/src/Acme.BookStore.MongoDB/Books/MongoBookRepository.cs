using System;
using Acme.BookStore.Books;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Acme.BookStore.MongoDB.Books;

public class MongoBookRepository : MongoDbRepository<BookStoreMongoDbContext, Book, Guid>
{
    public MongoBookRepository(IMongoDbContextProvider<BookStoreMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
