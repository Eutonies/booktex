using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.Zip;
public record BooktexZipFile(
    string BytesHash,
    IReadOnlyCollection<BooktexZipFileEntry> Entries,
    string? FileName = null
    );
