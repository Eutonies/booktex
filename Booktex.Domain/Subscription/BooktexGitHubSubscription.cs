using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.Subscription;
public record BooktexGitHubSubscription(
    long SubscriptionId,
    string GitHubOwner,
    string GitHubRepo
    ) : BooktexSubscription(SubscriptionId)
{

}
