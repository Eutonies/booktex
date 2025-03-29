using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using Booktex.Html.Common;
using Booktex.Html.Common.Style;
using Microsoft.AspNetCore.Components;

namespace Booktex.Html.Story;

public partial class StoryNarrationComponent
{


    [Parameter]
    public BookNarration Narration { get; set; }


    private string[] SplitForRendering => Narration.NarrationContent
    .Split('\n')
    .Select(_ => _.Replace("\r", ""))
    .ToArray();



}
