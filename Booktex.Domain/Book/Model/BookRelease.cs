namespace Booktex.Domain.Book.Model;

public record BookRelease(
    IReadOnlyCollection<BookChapter> Chapters,
    IReadOnlyDictionary<string, BookCharacterInfo> CharacterInfos,
    IReadOnlyDictionary<string, BookStoryLine> StoryLines,
    string Author,
    string Version,
    DateTime LastModified,
    BookAboutTheAuthor AboutTheAuthor
    )
{

}
