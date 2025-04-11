using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.Book.Specials.Model;
public record PsychLogQuote(
    string? MadeBy,
    string? Time,
    string? Place,
    string? Context,
    PsychLogEntryImportance? Importance,
    IReadOnlyCollection<string> Lines
    ) : PsychLogEntry(Importance ?? PsychLogEntryImportance.Normal);
