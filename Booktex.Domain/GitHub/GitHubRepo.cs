using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.GitHub;
public record GitHubRepo(
    string Owner,
    string Name
    )
{
    public static GitHubRepo From(string owner, string name) => new GitHubRepo(
        Owner: owner.ToLower().Trim(),
        Name: name.ToLower().Trim()
        );

    public GitHubRef Ref(string reference) => GitHubRef.From(this, reference);

}
