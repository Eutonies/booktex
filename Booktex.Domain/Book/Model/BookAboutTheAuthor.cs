using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.Book.Model;
public record BookAboutTheAuthor(
    IReadOnlyCollection<BookChapterContent> Contents
    );
