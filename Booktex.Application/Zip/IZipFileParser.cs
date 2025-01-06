using Booktex.Domain.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Application.Zip;
public interface IZipFileParser
{
    Task<BooktexZipFile?> Parse(byte[] fileBytes);
    Task<BooktexZipFile?> Parse(string fileName);


}
