using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.GitHub;
public record GitHubRef(
    GitHubRepo Repo,
    string ReferenceName
    )
{
    public static GitHubRef From(GitHubRepo repo, string reference) => new GitHubRef(repo, reference.ToLower().Trim());
    public static GitHubRef From(string owner, string repo, string reference) => From(GitHubRepo.From(owner,repo), reference);

}
