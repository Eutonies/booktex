using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booktex.Domain.Book.Model;
using ConTyp = Booktex.Persistence.Book.Model.BooktexBookChapterContentTypeDbo;

namespace Booktex.Persistence.Book.Model;
[Table(TableName)]
internal class BooktexBookChapterContentDbo
{
    public const string TableName = "book_chapter_content";
    [Column("content_id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ContentId { get; set; }

    [Column("about_the_author_id")]
    public long? AboutTheAuthorId { get; set; }

    [Column("chapter_id")]
    public long? ChapterId { get; set; }

    [Column("content_index")]
    public int Index { get; set; }

    [Column("content_type")]
    public ConTyp ContentType { get; set; }

    [Column("title_data")]
    public string? TitleData { get; set; }

    [Column("string_data")]
    public string? StringData { get; set; }

    [Column("is_thought")]
    public bool? IsThought { get; set; }
    [Column("is_numbered")]
    public bool? IsNumbered { get; set; }
    [Column("character_id")]
    public long? CharacterId { get; set; }

    public BookChapterSection ToSection() => new BookChapterSection(Title: TitleData!);

    public BookCharacterLine ToCharacterLine(
        BooktexBookCharacterDbo character,
        IEnumerable<BooktexBookChapterContentSubDbo> lineparts) => 
            new BookCharacterLine(
                Character: character.ToDomain(),
                LineParts: lineparts
                    .OrderBy(_ => _.Index)
                    .Select(lp => lp.ToCharacterLinePart())
                    .ToList(),
                IsThought: IsThought ?? false
            );

    public BookCharacterStoryTime ToStoryTime(BooktexBookCharacterDbo ch) =>
        new BookCharacterStoryTime(
            Character: ch.ToDomain(),
            Title: TitleData!,
            Story: StringData!
        );

    public BookContextBreak ToContextBreak() => new BookContextBreak();

    public BookNarration ToNarration() => new BookNarration(StringData!);

    public BookNarrationList ToNarrationList(IEnumerable<BooktexBookChapterContentSubDbo> subs) => 
        new BookNarrationList(
            Items: subs
                .OrderBy(_ => _.Index)
                .Select(_ => _.StringData!)
                .ToList(),
            IsNumbered: IsNumbered ?? false
        );

    public BookSinging ToBookSinging(BooktexBookCharacterDbo ch, IEnumerable<BooktexBookChapterContentSubDbo> subs) => new BookSinging(
        Character: ch.ToDomain(),
        LinesSong: subs
            .OrderBy(_ => _.Index)
            .Select(_ => _.StringData!)
            .ToList()
        );

    public void UpdateContentIds(IEnumerable<BooktexBookChapterContentSubDbo> subs)
    {
        foreach(var sub in subs)
        {
            sub.ContentId = ContentId;
        }
    }

}

internal static class BooktexBookChapterContentDboExtensions
{
    public static (BooktexBookChapterContentDbo Content, IReadOnlyCollection<BooktexBookChapterContentSubDbo> Subs, BooktexBookCharacterDbo? Character) ToDbo(
        this BookChapterContent cont, 
        long? characterId,
        int index,
        long? aboutTheAuthorId,
        long? chapterId) => cont switch
        {
            BookChapterSection sec => (new BooktexBookChapterContentDbo { TitleData = sec.Title, ContentType = ConTyp.ChapterSection, Index = index, AboutTheAuthorId = aboutTheAuthorId, ChapterId = chapterId }, [], null),
            BookCharacterLine lin => (
                Content: new BooktexBookChapterContentDbo
                {
                    CharacterId = characterId,
                    IsThought = lin.IsThought,
                    ContentType = ConTyp.CharacterLine,
                    Index = index,
                    AboutTheAuthorId = aboutTheAuthorId,
                    ChapterId = chapterId
                },
                Subs: lin.LineParts
                    .Select((lp, indx) => (Entry: lp, Indx: indx))
                    .Select(_ => _.Entry.ToDbo(contentId: 0L, _.Indx))
                    .ToList(),
                Character: lin.Character.ToDbo()
            ),
            BookCharacterStoryTime st => (
                Content: new BooktexBookChapterContentDbo
                {
                    TitleData = st.Title,
                    StringData = st.Story,
                    ContentType = ConTyp.CharacterStoryTime,
                    Index = index,
                    AboutTheAuthorId = aboutTheAuthorId,
                    ChapterId = chapterId
                },
                Subs: [],
                Character: st.Character.ToDbo()
             ),
            BookContextBreak cb => (new BooktexBookChapterContentDbo { ContentType = ConTyp.ContextBreak, Index = index, AboutTheAuthorId = aboutTheAuthorId, ChapterId = chapterId }, [], null),
            BookNarration narr => (
                Content: new BooktexBookChapterContentDbo {
                    ContentType = ConTyp.Narration,
                    StringData = narr.NarrationContent,
                    Index = index,
                    AboutTheAuthorId = aboutTheAuthorId,
                    ChapterId = chapterId },
                Subs: [],
                Character: null),
            BookNarrationList narrLis => (
                Content: new BooktexBookChapterContentDbo
                {
                    ContentType = ConTyp.NarrationList,
                    IsNumbered = narrLis.IsNumbered,
                    Index = index,
                    AboutTheAuthorId = aboutTheAuthorId,
                    ChapterId = chapterId
                },
                Subs: narrLis.Items
                    .Select((it, indx) => (Item: it, Indx: indx))
                    .Select(_ => new BooktexBookChapterContentSubDbo { Index = _.Indx, StringData = _.Item })
                    .ToList(),
                Character: null

             ),
        BookSinging sing => (
            Content: new BooktexBookChapterContentDbo
            {
                ContentType = ConTyp.BookSinging,
                CharacterId = characterId,
                Index = index,
                AboutTheAuthorId = aboutTheAuthorId,
                ChapterId = chapterId
            },
            Subs: sing.LinesSong
                .Select((it, indx) => (Item: it, Index: indx))
                .Select(_ => new BooktexBookChapterContentSubDbo { Index = _.Index, StringData = _.Item })
                .ToList(),
            Character: sing.Character.ToDbo()
        ),
        _ => throw new Exception()
    };
}
