namespace Booktex.Domain.Book.Model;
public record BookChapterMetaData(
    string? ChapterTitle = null,
    DateTime? ChapterDate = null,
    string? StoryLineKey = null,
    IReadOnlyDictionary<string, string>? Aliases = null,
    string? ChapterOrder = null
    );
