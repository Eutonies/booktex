using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.Book.Model;
public record BookReleaseShell(
    long ReleaseId,
    long SubscriptionId,
    string SubscriptionName,
    string Author,
    string Version,
    DateTime LastModified
    );
