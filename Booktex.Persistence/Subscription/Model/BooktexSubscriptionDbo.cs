using Booktex.Domain.Subscription;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Persistence.Subscription.Model;
[Table(TableName)]
internal class BooktexSubscriptionDbo
{
    public const string TableName = "subscription";
    [Column("subscription_id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SubscriptionId { get; set; }

    [Column("subscription_type")]
    public BooktexSubscriptionTypeDbo SubscriptionType { get; set; }

    [Column("github_owner")]
    public string? GitHubOwner { get; set; }

    [Column("github_repo")]
    public string? GitHubRepo { get; set; }

    [Column("file_regex")]
    public string? FileRegex { get; set; }

    public BooktexSubscription ToDomain() => SubscriptionType switch
    {
        BooktexSubscriptionTypeDbo.GitHub => ToGitHubSubscription(),
        _ => throw new InvalidOperationException()
    };

    public BooktexGitHubSubscription ToGitHubSubscription() => new BooktexGitHubSubscription(
        SubscriptionId: SubscriptionId,
        GitHubOwner: GitHubOwner!,
        GitHubRepo: GitHubRepo!,
        FileRegex: FileRegex
        );
}

internal static class BooktexSubscriptionDboExtensions
{
    public static BooktexSubscriptionDbo ToDbo(this BooktexSubscription sub) => sub switch
    {
        BooktexGitHubSubscription ghSub => new BooktexSubscriptionDbo
        {
            SubscriptionId = ghSub.SubscriptionId,
            SubscriptionType = BooktexSubscriptionTypeDbo.GitHub,
            GitHubOwner = ghSub.GitHubOwner,
            GitHubRepo = ghSub.GitHubRepo,
            FileRegex = ghSub.FileRegex
        },
        _ => throw new InvalidOperationException()
    };
}
