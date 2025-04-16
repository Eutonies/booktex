using Booktex.Domain.Book.Model;
using Booktex.Domain.Book.Specials;
using Booktex.Domain.Util;
using Microsoft.AspNetCore.Components;
using System.Text;

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

    private static MarkupString TextFor(BookCharacterLinePart part)
    {
        if(part.PartText.Contains(SpecialTexts.FastRollingR) || part.PartText.Contains(SpecialTexts.SlowRollingR))
        {
            return ProduceRollingRs(part);
        }
        return new MarkupString(part.PartText);
    }

    private static MarkupString ProduceRollingRs(BookCharacterLinePart part)
    {
        var replacedText = part.PartText
            .Replace(SpecialTexts.FastRollingR, FastRollingR)
            .Replace(SpecialTexts.SlowRollingR, SlowRollingR);
        var returnee = $@"<div class=""animation-text"" {IsolationId}>{replacedText}</div>";
        return new MarkupString(returnee);
    }

    private const string IsolationId = "story-character-line";
    private static readonly string FastRollingR = $@"<div class=""rolling-r"" {IsolationId}><div class=""rolling-r-fast"" {IsolationId}>r</div></div>";
    private static readonly string SlowRollingR = $@"<div class=""rolling-r"" {IsolationId}><div class=""rolling-r-slow"" {IsolationId}>r</div></div>";

}
