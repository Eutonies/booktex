using Booktex.Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.Book.Specials.Model;
public enum PsychLogEntryImportance
{
    Low = 10,
    Normal = 20,
    High = 30,
    VeryHigh = 40
}


public static class PsychLogEntryImportanceExtensions
{
    private static readonly IReadOnlyDictionary<string, PsychLogEntryImportance> Importances = new List<PsychLogEntryImportance>
    {
        PsychLogEntryImportance.Low, PsychLogEntryImportance.Normal, PsychLogEntryImportance.High, PsychLogEntryImportance.VeryHigh
    }.Select(_ => (_.ToString().ToLower(), _))
        .ToDictionarySafe(_ => _.Item1, _ => _.Item2);
    public static PsychLogEntryImportance ParseAsImportance(this string imp) => Importances.TryGetValue(imp.ToLower().Trim(), out var impo) ? impo : PsychLogEntryImportance.Normal;
}