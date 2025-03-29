using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using Booktex.Html.Common.Style;
using Microsoft.AspNetCore.Components;

namespace Booktex.Html.Story;

public partial class StoryComponent
{

    [Parameter]
    public IReadOnlyCollection<BookChapterContent> Contents { get; set; }

    [Parameter]
    public string ChapterName { get; set; }

    [Parameter]
    public string ChapterSubName { get; set; }

    [Parameter]
    public int ZIndex { get; set; } = 100;

    [Parameter]
    public Func<BookQuote,BooktexBackgroundImageSpecification?>? BackgroundDeriver { get; set; }

    private BooktexBackgroundImageSpecification? BackgroundFor(BookQuote quote) => BackgroundDeriver?
        .Pipe(der => der(quote));


}
