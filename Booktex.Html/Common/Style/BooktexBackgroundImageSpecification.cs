namespace Booktex.Html.Common.Style;
public record BooktexBackgroundImageSpecification(
    string? BackgroundImage = null,
    string? BackgroundSize = null,
    string? BackgroundRepeat = null,
    string? BackgroundPositionX = null,
    string? BackgroundPositionY = null
    ) : BooktexStyleable()
{
    public override IReadOnlyCollection<(string Name, string? Value)> CssRules => [
        ("background-image", BackgroundImage),
        ("background-size", BackgroundSize),
        ("background-repeat", BackgroundRepeat),
        ("background-position-x", BackgroundPositionX),
        ("background-position-y", BackgroundPositionY)
    ];


}