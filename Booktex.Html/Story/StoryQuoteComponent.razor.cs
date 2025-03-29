using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using Booktex.Html.Common;
using Booktex.Html.Common.Style;
using Microsoft.AspNetCore.Components;

namespace Booktex.Html.Story;

public partial class StoryQuoteComponent
{
    private const string ComponentName = "quote";
    private const string ImageContainerSuffix = "bg";
    private readonly string _id = ComponentId.Next();

    private string? BackgroundImageClass => ImageSpec?.Pipe(_ => ComponentName.ClassName(_id, ImageContainerSuffix));

    [Parameter]
    public BookQuote Quote { get; set; }

    [Parameter]
    public BooktexBackgroundImageSpecification? ImageSpec { get; set; }

    private string? QuoteClass => (Quote.Name?.ToLower()?.Trim(), Quote.SubName?.ToLower()?.Trim()) switch
    {
        (string nam, _) when nam.Contains("cold war") => "quot-cold-war",
        _ => null
    };


}
