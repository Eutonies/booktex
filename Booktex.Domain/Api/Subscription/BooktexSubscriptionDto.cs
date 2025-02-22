using Booktex.Domain.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.Api.Subscription;
public record BooktexSubscriptionDto(
    long SubscriptionId,
    BooktexSubscriptionTypeDto SubscriptionType,
    string? GitHubOwner,
    string? GitHubRepo,
    string? FileRegex
    )
{
    public BooktexGitHubSubscription ToDomain() => SubscriptionType switch
    {
        BooktexSubscriptionTypeDto.GitHub => ToDomainGithub(),
        _ => throw new NotImplementedException()
    };

    public BooktexGitHubSubscription ToDomainGithub() => new BooktexGitHubSubscription(
        SubscriptionId: SubscriptionId,
        GitHubOwner: GitHubOwner!,
        GitHubRepo: GitHubRepo!,
        FileRegex: FileRegex
       );
}


public static class BooktexSubscriptionDtoExtensions
{
    public static BooktexSubscriptionDto ToDto(this BooktexSubscription sub) => sub switch
    {
        BooktexGitHubSubscription ghSub => new BooktexSubscriptionDto(
            SubscriptionId: sub.SubscriptionId,
            SubscriptionType: BooktexSubscriptionTypeDto.GitHub,
            GitHubOwner: ghSub.GitHubOwner,
            GitHubRepo: ghSub.GitHubRepo,
            FileRegex: ghSub.FileRegex
          ),
        _ => throw new NotImplementedException()

    };
}
        
        
 
