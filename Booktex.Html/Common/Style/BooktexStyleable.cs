using Booktex.Domain.Util;
using Microsoft.AspNetCore.Components;

namespace Booktex.Html.Common.Style;
public abstract record BooktexStyleable
{
    public MarkupString ToCssMarkup(string componentName, string componentId, string? suffix = null) => ToCssStyle()
        .Pipe(styl => styl.InCssClass(componentName, componentId, suffix))
        .Pipe(cls => new MarkupString( "." + cls));

    public MarkupString ToCssMarkup(string className) => ToCssStyle()
        .InCssClass(className)
        .Pipe(_ => new MarkupString("." + _));

    public abstract IReadOnlyCollection<(string Name, string? Value)> CssRules { get; }


    string ToCssStyle() => CssRules.Where(_ => _.Value != null)
     .Select(p => $"  {p.Name}: {p.Value};")
     .MakeString("\n");



}
