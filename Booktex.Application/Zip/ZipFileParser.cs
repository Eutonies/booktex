using Booktex.Domain.Util;
using Booktex.Domain.Zip;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Application.Zip;
internal class ZipFileParser : IZipFileParser
{
    private static readonly HashSet<string> _allowableFileTypes = new List<string>
    {
        "story",
        "ata",
        "json"
    }.ToHashSet();

    public async Task<BooktexZipFile?> Parse(string fileName)
    {
        var fileBytes = await File.ReadAllBytesAsync(fileName);
        var returnee = (await Parse(fileBytes)) switch
        {
            null => null,
            BooktexZipFile zf => zf with { FileName = fileName }
        };
        return returnee;
    }

    public async Task<BooktexZipFile?> Parse(byte[] fileBytes)
    {
        using var memStream = new MemoryStream(fileBytes);
        var zipEntry = new ZipArchive(memStream, ZipArchiveMode.Read);
        var entries = new List<BooktexZipFileEntry>();
        var relevantEntries = zipEntry.Entries
            .Where(_ => _allowableFileTypes.Contains(_.Name.ToLower().AfterLastIndexOf(".")))
            .ToList();
        foreach (var entry in relevantEntries)
        {
            using var outStream = new MemoryStream();
            using (var fileEntStream = entry.Open())
            {
                await fileEntStream.CopyToAsync(outStream);
            }
            var fileContent = outStream.ToArray();
            var stringContent = Encoding.UTF8.GetString(fileContent);
            entries.Add(new BooktexZipFileEntry(entry.FullName, stringContent));
        }
        if (entries.Count > 0)
        {
            var hashCode = entries
                .OrderBy(_ => _.Filename)
                .AggregateHash("", _ => _.Content);
            var returnee = new BooktexZipFile(hashCode, entries);
            return returnee;
        }
        return null;
    }
}
