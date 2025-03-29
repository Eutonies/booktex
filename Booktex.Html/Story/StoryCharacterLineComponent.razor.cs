using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using Microsoft.AspNetCore.Components;

namespace Booktex.Html.Story;

public partial class StoryCharacterLineComponent
{
    [Parameter]
    public BookCharacterLine Line { get; set; }

    [Parameter]
    public bool IsRight { get; set; }

    [Parameter]
    public BookInteractionType? InteractionType { get; set; }

    private string? InteractionClass => (InteractionType ?? Line.InteractionType) switch
    {
        null => null,
        BookInteractionType.PhoneCall => "int-phone",
        BookInteractionType.SMS => "int-sms",
        _ => null
    };

}
