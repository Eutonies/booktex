using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Infrastructure.Configuration;
public class InfrastructureConfiguration
{
    public const string ConfigurationElementName = "Infrastructure";
    public GitHubConfiguration GitHub { get; set; }

}
