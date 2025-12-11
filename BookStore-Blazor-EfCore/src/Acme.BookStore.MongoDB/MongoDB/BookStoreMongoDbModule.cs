using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Acme.BookStore.MongoDB;

[DependsOn(
    typeof(BookStoreDomainModule),
    typeof(AbpMongoDbModule)
)]
public class BookStoreMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<BookStoreMongoDbContext>(options =>
        {
            options.AddDefaultRepositories(includeAllEntities: true);
            
            // Add custom repositories
            options.AddRepository<Book, Books.MongoBookRepository>();
            options.AddRepository<Author, Authors.MongoAuthorRepository>();
        });
    }
}
