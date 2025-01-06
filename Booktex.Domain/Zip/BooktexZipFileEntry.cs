using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.Zip;
public record BooktexZipFileEntry(
    string Filename,
    string Content
    );
