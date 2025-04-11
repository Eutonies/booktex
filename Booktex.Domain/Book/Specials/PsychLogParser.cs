using Booktex.Domain.Book.Specials.Model;
using Booktex.Domain.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Booktex.Domain.Book.Specials;
public static class PsychLogParser
{
    public const string EntryTypeObservation = "observation";
    public const string EntryTypeReflection = "reflection";
    public const string EntryTypeQuote = "quote";


    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true
        
    };


    public static PsychLog Parse(string content)
    {
        var read = JsonSerializer.Deserialize<ParseLog>(content, JsonOptions)!;
        var entries = read.Entries
            .Select(ent => ent.Content
               .Select(_ => _.Trim())
               .ToList()
               .Pipe<List<string>, PsychLogEntry>(contLis => ent.EntryType.ToLower().Trim() switch
                    {
                        EntryTypeObservation => new PsychLogObservation(contLis, ent.Context, ent.Importance?.ParseAsImportance()),
                        EntryTypeReflection => new PsychLogReflection(contLis, ent.Context, ent.Importance?.ParseAsImportance()),
                        EntryTypeQuote => new PsychLogQuote(ent.MadeBy, ent.Time, ent.Place, ent.Context, ent.Importance?.ParseAsImportance(), contLis),
                        _ => throw new Exception("Cannot parse: " + ent)
                    })
            ).ToList();
        var returnee = new PsychLog(
            Date: DateTime.ParseExact(read.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Time: read.Time,
            Place: read.Place,
            PatientNo: read.PatientNo,
            PatientName: read.PatientName,
            Entries: entries
            );
        return returnee;

    }




    private record ParseLog(
        string Date,
        string? Time,
        string? Place,
        string PatientNo,
        string? PatientName,
        IReadOnlyCollection<ParseEntry> Entries
    );

    private record ParseEntry(
        string EntryType,
        string? Importance,
        IReadOnlyCollection<string> Content,
        string? MadeBy,
        string? Time,
        string? Place,
        string? Context
        );

}
