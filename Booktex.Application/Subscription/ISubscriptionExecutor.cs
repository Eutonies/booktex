using Booktex.Domain.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Application.Subscription;
public interface ISubscriptionExecutor
{
    Task<IReadOnlyCollection<BooktexSubscriptionExecutionFile>> ObtainFilesForSubscriptionExecution(BooktexSubscription sub); 

}
