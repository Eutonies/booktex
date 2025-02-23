using Booktex.Domain.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Application.Subscription;
public interface ISubscriptionRepo
{
    Task<IReadOnlyCollection<BooktexSubscription>> LoadSubscriptions();

    Task<BooktexGitHubSubscription> Create(BooktexGitHubSubscription subscription);

    Task<BooktexSubscription?> Exists(string githubOwner, string githubRepo, string? fileRegex);

    Task<BooktexGitHubSubscription> Update(BooktexGitHubSubscription subscription);

    Task<BooktexSubscriptionExecution> Create(long subscriptionId, BooktexSubscriptionExecution execution);



}
