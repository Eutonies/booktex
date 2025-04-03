using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using Booktex.Html.Common;
using Booktex.Html.Common.Style;
using Microsoft.AspNetCore.Components;

namespace Booktex.Html.Story;

public partial class StorySectionComponent
{

    [Parameter]
    public BookChapterSection Section { get; set; }


}
