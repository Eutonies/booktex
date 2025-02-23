using Booktex.Application.GitHub;
using Booktex.Domain.GitHub;
using Booktex.Domain.Subscription;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Application.Subscription;
internal class SubscriptionExecutor : ISubscriptionExecutor
{

    private readonly IServiceScopeFactory _scopeFactory;

    public SubscriptionExecutor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<IReadOnlyCollection<BooktexSubscriptionExecutionFile>> ObtainFilesForSubscriptionExecution(BooktexSubscription sub)
    {
        if (sub is not BooktexGitHubSubscription ghSub)
            return [];

        using var scope = _scopeFactory.CreateScope();
        var githubLoader = scope.ServiceProvider.GetRequiredService<IGitHubService>();
        var repo = new GitHubRepo(ghSub.GitHubOwner, ghSub.GitHubRepo);
        var ghRef = new GitHubRef(repo, "main");
        var downloadedBytes = await githubLoader.DownloadFiles(ghRef);
        using var byteStream = new MemoryStream(downloadedBytes);
        using var zipFile = new ZipArchive(byteStream);
        var files = zipFile.Entries
            .Select(_ => new BooktexSubscriptionExecutionFile(
                FileId: 0L,
                ExecutionId: 0L,
                AbsoluteFileName: _.Name,
                FileContent: ReadEntryContent(_)
            ))
            .ToList();
        return files;
    }

    private string ReadEntryContent(ZipArchiveEntry ent)
    {
        var task = Task.Run(async () =>
        {
            var byts = await ReadEntry(ent);
            return UTF8Encoding.UTF8.GetString(byts);
        });
        task.Wait();
        return task.Result;
    }

    private async Task<byte[]> ReadEntry(ZipArchiveEntry ent)
    {
        using var readStream = ent.Open();
        using var buffer = new MemoryStream();
        await readStream.CopyToAsync(buffer);
        var returnee = buffer.ToArray();
        return returnee;
    }

}
