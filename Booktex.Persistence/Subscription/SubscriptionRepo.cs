using Booktex.Application.Subscription;
using Booktex.Domain.Subscription;
using Booktex.Persistence.Context;
using Booktex.Persistence.Subscription.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Persistence.Subscription;
internal class SubscriptionRepo : ISubscriptionRepo
{
    private readonly IDbContextFactory<BooktexDbContext> _contextFactory;

    public SubscriptionRepo(IDbContextFactory<BooktexDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<BooktexGitHubSubscription> Create(BooktexGitHubSubscription subscription)
    {
        await using var cont = await _contextFactory.CreateDbContextAsync();
        var existingQuery = cont.Subscriptions
            .AsNoTracking()
            .Where(_ => _.SubscriptionType == Model.BooktexSubscriptionTypeDbo.GitHub)
            .Where(_ => _.GitHubOwner == subscription.GitHubOwner && _.GitHubRepo == subscription.GitHubRepo);
        if(subscription.FileRegex != null)
            existingQuery = existingQuery.Where(_ => _.FileRegex == subscription.FileRegex);
        else
            existingQuery = existingQuery.Where(_ => _.FileRegex == null);
        var existing = await existingQuery.FirstOrDefaultAsync();
        if (existing != null)
        {
            var returnee = existing.ToGitHubSubscription();
            return returnee;
        }
        var insertee = subscription.ToDbo();
        cont.Add(insertee);
        await cont.SaveChangesAsync();
        var ret = insertee.ToGitHubSubscription();
        return ret;
    }

    public async Task<BooktexSubscriptionExecution> Create(long subscriptionId, BooktexSubscriptionExecution execution)
    {
        await using var cont = await _contextFactory.CreateDbContextAsync();
        var existing = await cont.Executions
            .AsNoTracking()
            .Where(_ => _.SubscriptionId == subscriptionId)
            .Where(_ => _.HashCode == execution.HashCode)
            .FirstOrDefaultAsync();
        if (existing != null)
            return await Load(existing.ExecutionId, cont);
        var insertee = execution.ToDbo();
        insertee.SubscriptionId = subscriptionId;
        cont.Add(insertee);
        await cont.SaveChangesAsync();
        var files = execution.Files
            .Select(_ => _ with { ExecutionId = insertee.ExecutionId})
            .OrderBy(_ => _.AbsoluteFileName)
            .Select(_ => _.ToDbo())
            .ToList();
        cont.AddRange(files);
        await cont.SaveChangesAsync();
        var returnee = await Load(insertee.ExecutionId, cont);
        return returnee;
    }

    public async Task<long?> Exists(string githubOwner, string githubRepo, string? fileRegex)
    {
        await using var cont = await _contextFactory.CreateDbContextAsync();

        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<BooktexSubscription>> LoadSubscriptions()
    {
        await using var cont = await _contextFactory.CreateDbContextAsync();
        var loaded = await cont.Subscriptions
            .AsNoTracking()
            .ToListAsync();
        var returnee = loaded
            .Select(_ => _.ToDomain())
            .OrderBy(_ => _.SubscriptionId)
            .ToList();
        return returnee;
    }

    public async Task<BooktexGitHubSubscription> Update(BooktexGitHubSubscription subscription)
    {
        using var cont = await _contextFactory.CreateDbContextAsync();
        var existing = await cont.Subscriptions
            .Where(_ => _.SubscriptionId == subscription.SubscriptionId)
            .FirstOrDefaultAsync();
        if (existing != null)
        {
            existing.GitHubOwner = subscription.GitHubOwner;
            existing.GitHubRepo = subscription.GitHubRepo;
            existing.FileRegex = subscription.FileRegex;
            cont.Update(existing);
            await cont.SaveChangesAsync();
            return existing.ToGitHubSubscription();
        }
        return subscription;
    }

    private async Task<BooktexSubscriptionExecution> Load(long executionId, BooktexDbContext cont)
    {
        var execution = await cont.Executions
            .AsNoTracking()
            .FirstAsync(_ => _.ExecutionId == executionId);
        var files = await cont.ExecutionFiles
            .AsNoTracking()
            .Where(_ => _.ExecutionId == executionId)
            .ToListAsync();
        var returnee = execution.ToDomain(files);
        return returnee;
    }

}
