using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Domain.Book.Model;
public record BookDialog(
    BookCharacter LeftSide,
    BookCharacter RightSide,
    IReadOnlyCollection<BookDialogEntry> Entries
    ) : BookChapterContent()
{
}

public abstract record BookDialogEntry(
    BookCharacterLine Line
    )
{
    public static BookDialogEntry Left(BookCharacterLine line) => new BookDialogLeftSideEntry(line);
    public static BookDialogEntry Right(BookCharacterLine line) => new BookDialogRightSideEntry(line);

    public BookCharacterLine? LeftSide => this switch
    {
            BookDialogLeftSideEntry ent => ent.Line,
            _ => null
    };

    public BookCharacterLine? RightSide => this switch
    {
        BookDialogRightSideEntry ent => ent.Line,
        _ => null
    };
}
public record BookDialogLeftSideEntry(BookCharacterLine Line) : BookDialogEntry(Line);

public record BookDialogRightSideEntry(BookCharacterLine Line) : BookDialogEntry(Line);
