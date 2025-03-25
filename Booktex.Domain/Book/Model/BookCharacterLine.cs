namespace Booktex.Domain.Book.Model;
public record BookCharacterLine(
    BookCharacter Character,
    IReadOnlyCollection<BookCharacterLinePart> LineParts,
    bool IsThought = false,
    BookInteractionType? InteractionType = null
    ) : BookChapterContent;
