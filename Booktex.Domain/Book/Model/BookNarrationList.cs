namespace Booktex.Domain.Book.Model;

public record BookNarrationList(IReadOnlyCollection<string> Items, bool IsNumbered) : BookChapterContent;