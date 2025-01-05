namespace Booktex.Domain.Book.Model;

public record BookNarration(
    string NarrationContent
    ) : BookChapterContent;
