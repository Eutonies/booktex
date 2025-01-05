namespace Booktex.Domain.Book.Model;

public record BookSinging(
    BookCharacter Character,
    IReadOnlyCollection<string> LinesSong
    ) : BookChapterContent
{
}
