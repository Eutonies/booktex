using Booktex.Domain.GitHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Application.GitHub;
public interface IGitHubService
{
    Task<byte[]> DownloadFiles(GitHubRef reference);

}
