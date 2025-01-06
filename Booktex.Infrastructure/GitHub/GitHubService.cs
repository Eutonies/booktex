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
    private readonly GitHubConfiguration _conf;

    public GitHubService(IOptions<InfrastructureConfiguration> conf)
    {
        _conf = conf.Value.GitHub;
    }

    public async Task<byte[]> DownloadFiles(GitHubRef reference)
    {
        var (repo, refName) = reference;
        try
        {
            var octoClient = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("booktex"));
            octoClient.Credentials = new Octokit.Credentials(_conf.FineGrainedToken, Octokit.AuthenticationType.Bearer);
            var result = await octoClient.Repository.Content.GetArchive(repo.Owner, repo.Name, Octokit.ArchiveFormat.Zipball, refName);
            return result;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

}
