using Booktex.Application.GitHub;
using Booktex.Domain.GitHub;
using Booktex.Infrastructure.Configuration;
using GitHub;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SysFile = System.IO.File;

namespace Booktex.Infrastructure.GitHub;
public class GitHubService : IGitHubService
{
    private readonly IGitHubApiClient _client;
    private readonly GitHubConfiguration _conf;

    public GitHubService(IGitHubApiClient client, IOptions<InfrastructureConfiguration> conf)
    {
        _client = client;
        _conf = conf.Value.GitHub;

    }

    public async Task DownloadFiles(GitHubRef reference)
    {
        var (repo, refName) = reference;
        await _client.Repos_downloadZipballArchiveAsync(repo.Owner, repo.Name, refName);
    }

}
