using Booktex.Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.Subscription;
public record BooktexSubscriptionExecutionFile(
    long FileId,
    long ExecutionId,
    string AbsoluteFileName,
    string FileContent
    )
{
    public string FileName = AbsoluteFileName.AfterLastIndexOf("/").AfterLastIndexOf("\\");

}
