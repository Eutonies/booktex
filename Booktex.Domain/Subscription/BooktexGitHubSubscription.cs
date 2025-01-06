using Booktex.Domain.Util;
using Booktex.Domain.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Booktex.Domain.Subscription;
public record BooktexGitHubSubscription(
    long SubscriptionId,
    string GitHubOwner,
    string GitHubRepo,
    string? FileRegex
    ) : BooktexSubscription(SubscriptionId)
{
    private Regex? _compiledFileRegex;
    private Regex? CompiledFileRegex => _compiledFileRegex ??= FileRegex?.Pipe(regStr => new Regex(regStr));
    public IReadOnlyCollection<A> Filter<A>(IEnumerable<A> inputs, Func<A, string> nameExtractor) => (CompiledFileRegex switch
    {
        null => inputs,
        Regex reg => inputs.Where(fil => reg.IsMatch(nameExtractor(fil)))
    }).ToList();

    public IReadOnlyCollection<BooktexSubscriptionExecutionFile> Filter(IEnumerable<BooktexSubscriptionExecutionFile> files) =>
        Filter(files, _ => _.AbsoluteFileName);

    public IReadOnlyCollection<BooktexZipFileEntry> Filter(IEnumerable<BooktexZipFileEntry> files) =>
        Filter(files, _ => _.Filename);

    public BooktexSubscriptionExecution ExecutionFrom(BooktexZipFile zipFile) => new BooktexSubscriptionExecution(
        ExecutionId: 0L,
        SubscriptionId: SubscriptionId,
        ExecutionTime: DateTime.Now,
        Files: Filter(zipFile.Entries)
           .Select(fil => new BooktexSubscriptionExecutionFile(
               FileId: 0L,
               ExecutionId: 0L,
               AbsoluteFileName: fil.Filename,
               FileContent: fil.Content
               ))
           .ToList()
        );

}
