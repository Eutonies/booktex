using Booktex.Application.Book;
using Booktex.Domain.Book.Model;
using Booktex.Persistence.Book.Model;
using Booktex.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Booktex.Persistence.Book;
internal class BookRepo : IBookRepo
{
    private readonly IDbContextFactory<BooktexDbContext> _contextFactory;

    public BookRepo(IDbContextFactory<BooktexDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IReadOnlyCollection<BookReleaseShell>> LoadReleases(long? subscriptionId)
    {
        await using var cont = await _contextFactory.CreateDbContextAsync();
        var releaseQuery = cont.BookReleases
            .AsNoTracking();
        if (subscriptionId != null)
            releaseQuery = releaseQuery.Where(_ => _.SubscriptionId == subscriptionId.Value);
        var releasesWithSubQuery = from rel in releaseQuery
                                   join sub in cont.Subscriptions
                                   on rel.SubscriptionId equals sub.SubscriptionId
                                   select new { Release = rel, Subscription = sub };
        var releasesWithSub = await releasesWithSubQuery.ToListAsync();
        var returnee = releasesWithSub
            .Select(relPair => new BookReleaseShell(
                ReleaseId: relPair.Release.ReleaseId,
                SubscriptionId: relPair.Subscription.SubscriptionId,
                SubscriptionName: $"{relPair.Subscription.SubscriptionType.ToString()}: {relPair.Subscription.GitHubOwner} - {relPair.Subscription.GitHubRepo}",
                Author: relPair.Release.Author,
                Version: relPair.Release.Version,
                LastModified: relPair.Release.LastModified
               ))
            .ToList();
        return returnee;
    }

    public async Task SaveRelease(long subscriptionId, BookRelease release)
    {
        await using var cont = await _contextFactory.CreateDbContextAsync();
        var releaseDbo = release.ToDbo(subscriptionId);
        cont.Add(releaseDbo);
        await cont.SaveChangesAsync();
        var aboutTheAuthor = release.AboutTheAuthor.ToDbo();
        releaseDbo.UpdateWithReleaseId(aboutTheAuthor);
        cont.Add(aboutTheAuthor);
        await cont.SaveChangesAsync(); 

    }

    private async Task SaveContents(BooktexDbContext cont, long? chapterId, long? aboutTheAuthorId, IEnumerable<BookChapterContent> contents)
    {
        var contentDbos = contents
            .Select((con, indx) => (Content: con, Index: indx, Character: con.ExtractCharacter()))
            .ToList();

    }

    private async Task SaveContentsNa(BooktexDbContext cont, long? chapterId, long? aboutTheAuthorId, IEnumerable<BookChapterContent> contents)
    {
        var contentDbos = contents
            .Select((con, indx) => (Content: con, Index: indx, Character: con.ExtractCharacter()))
            .ToList();
        await Task.CompletedTask;
    }



}

internal static class BookRepoExtensions
{
    public static BooktexBookCharacterDbo? ExtractCharacter(this BookChapterContent cont) => cont switch
    {
        BookCharacterLine lin => lin.Character.ToDbo(),
        BookCharacterStoryTime st => st.Character.ToDbo(),
        BookSinging sin => sin.Character.ToDbo(),
        _ => null
    };


}

