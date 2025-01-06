using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.Util;
public static class StringExtensions
{

    public static string ToShaHash(this string str) => Encoding.UTF8.GetBytes(str).ToShaHash();

    public static string CombineHash(this string alreadyHashed, string? nextStr) => $"{alreadyHashed}{nextStr ?? ""}".ToShaHash();

    public static string ToBase64(this string hash) => Convert.ToBase64String(Encoding.UTF8.GetBytes(hash));


    public static string AggregateHash<TItem>(this IEnumerable<TItem> items, string initial, Func<TItem, string> stringer) => 
        items.Any() ? 
        items.Aggregate(initial.ToShaHash(), (hashed, snd) => hashed.CombineHash(stringer(snd).ToShaHash())) :
        initial.ToShaHash();

    public static string AfterLastIndexOf(this string input, string toOccur)
    {
        var lastIndex = input.LastIndexOf(toOccur);
        if(lastIndex > 0)
        {
            return input.Substring(lastIndex + toOccur.Length);
        }
        return input;
    }

    public static byte[] Compress(this string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        using var ms = new MemoryStream();
        using (var gzStream = new GZipStream(ms, CompressionLevel.Optimal))
        {
            gzStream.Write(bytes, 0, bytes.Length);
        }
        return ms.ToArray();
    }
    public static string DeCompress(this byte[] input)
    {

        using var ms = new MemoryStream(input);
        using var outStream = new MemoryStream();
        using (var gzStream = new GZipStream(ms, CompressionMode.Decompress))
        {
            gzStream.CopyTo(outStream);
        }
        var decompressedBytes = outStream.ToArray();
        var returnee = Encoding.UTF8.GetString(decompressedBytes);
        return returnee;
    }



}
