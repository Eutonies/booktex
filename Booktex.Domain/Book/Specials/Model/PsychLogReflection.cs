using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.Book.Specials.Model;
public record PsychLogReflection(
    IReadOnlyCollection<string> Lines,
    string? Context,
    PsychLogEntryImportance? Importance
    ) : PsychLogEntry(Importance ?? PsychLogEntryImportance.Normal)

{
}
