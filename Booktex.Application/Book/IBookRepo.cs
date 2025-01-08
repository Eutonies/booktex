using Booktex.Domain.Book.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Application.Book;
public interface IBookRepo
{
    Task SaveRelease(long subscriptionId, BookRelease release);
    Task<IReadOnlyCollection<BookReleaseShell>> LoadReleases(long? subscriptionId);

}
