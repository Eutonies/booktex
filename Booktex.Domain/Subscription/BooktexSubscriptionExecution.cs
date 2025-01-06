using Booktex.Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.Subscription;
public record BooktexSubscriptionExecution(
    long ExecutionId,
    long SubscriptionId,
    DateTime ExecutionTime,
    IReadOnlyCollection<BooktexSubscriptionExecutionFile> Files
    )
{
    private string? _hashCode;
    public string HashCode => _hashCode ??= Files
        .OrderBy(_ => _.AbsoluteFileName)
        .AggregateHash(nameof(BooktexSubscriptionExecution), _ => _.FileContent);

}
