using Booktex.Domain.Util;
using Booktex.Html.Common.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Html.Common;
public static class CssExtensions
{
    public static string InCssClass(this string style, string componentName, string componentId, string? suffix = null) =>
        style.InCssClass(ClassName(componentName, componentId, suffix));

    public static string InCssClass(this string style, string className) =>
    $"{className} {{\n{style}\n}}";


    public static string ClassName(this string componentName, string componentId, string? suffix = null) =>
        $".{componentName}-{componentId}{suffix?.Pipe(suff => $"-{suff}")}";

    public static string ResRef(this string qualifier) => 
        $"_content/{nameof(Booktex)}.{nameof(Booktex.Html)}/{qualifier}";

}
