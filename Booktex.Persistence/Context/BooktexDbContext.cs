using Booktex.Application.Book;
using Booktex.Persistence.Book;
using Booktex.Persistence.Book.Model;
using Booktex.Persistence.Subscription.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Persistence.Context;
internal class BooktexDbContext : DbContext
{

    public DbSet<BooktexSubscriptionDbo> Subscriptions { get; set; }
    public DbSet<BooktexSubscriptionExecutionDbo> Executions { get; set; }
    public DbSet<BooktexSubscriptionExecutionFileDbo> ExecutionFiles { get; set; }


    public DbSet<BooktexBookAboutTheAuthorDbo> BookAboutTheAuthors { get; set; }
    public DbSet<BooktexBookChapterContentDbo> BookChapterContents { get; set; }
    public DbSet<BooktexBookChapterContentSubDbo> BookChapterContentSubs { get; set; }
    public DbSet<BooktexBookChapterDbo> BookChapters { get; set; }
    public DbSet<BooktexBookChapterMetadataDbo> BookMetaDatas { get; set; }
    public DbSet<BooktexBookChapterMetadataMappingDbo> BookMetaDataMappings { get; set; }
    public DbSet<BooktexBookCharacterDbo> BookCharacters { get; set; }
    public DbSet<BooktexBookReleaseDbo> BookReleases { get; set; }
    public DbSet<BooktexBookStoryLineDbo> BookStoryLines { get; set; }

    public BooktexDbContext(DbContextOptions<BooktexDbContext> options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<DateTime?>().HaveColumnType("timestamp without time zone");
        configurationBuilder.Properties<DateTime>().HaveColumnType("timestamp without time zone");
        configurationBuilder.Properties<byte[]>().HaveColumnType("bytea");
    }


}
