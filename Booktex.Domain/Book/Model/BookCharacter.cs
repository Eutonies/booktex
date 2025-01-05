namespace Booktex.Domain.Book.Model;
public record BookCharacter(
    string CharacterName,
    BookCharacterInfo? CharacterInfo,
    string? Alias
    )
{
    public string CharacterKey = CharacterName.Trim().ToLower();
    public string CharacterFormatKey => Alias?.Trim()?.ToLower() ?? CharacterKey;

    public string CharacterAudioKey => CharacterFormatKey;
}
