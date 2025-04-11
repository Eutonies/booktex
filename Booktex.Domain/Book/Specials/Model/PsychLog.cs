using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.Book.Specials.Model;
public record PsychLog(
    DateTime Date,
    string? Time,
    string? Place,
    string PatientNo,
    string? PatientName,
    IReadOnlyCollection<PsychLogEntry> Entries
    );
