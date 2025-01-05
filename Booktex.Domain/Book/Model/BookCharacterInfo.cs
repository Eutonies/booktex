namespace Booktex.Domain.Book.Model;
public record BookCharacterInfo(
    string Character,
    string ShowName,
    string Color,
    string? Font,
    string? SiteFont
    )
{
    public string CharacterKey = Character.Trim().ToLower();


}
