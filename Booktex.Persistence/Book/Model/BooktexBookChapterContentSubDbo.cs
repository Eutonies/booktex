using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booktex.Domain.Book.Model;

namespace Booktex.Persistence.Book.Model;
[Table(TableName)]
internal class BooktexBookChapterContentSubDbo
{
    public const string TableName = "book_chapter_content_sub";
    [Column("content_sub_id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ContentSubId { get; set; }

    [Column("content_id")]
    public long ContentId { get; set; }

    [Column("string_data")]
    public string? StringData { get; set; }

    [Column("string_data_description")]
    public string? StringDataDescription { get; set; }

    [Column("content_sub_index")]
    public int Index { get; set; }

    public BookCharacterLinePart ToCharacterLinePart() => new BookCharacterLinePart(
        PartText: StringData!,
        Description: StringDataDescription
        );


}


internal static class BooktexBookChapterContentSubDboExtensions
{
    public static BooktexBookChapterContentSubDbo ToDbo(this BookCharacterLinePart lp, long contentId, int index) => new BooktexBookChapterContentSubDbo
    {
        ContentId = contentId,
        StringData = lp.PartText,
        StringDataDescription = lp.Description,
        Index = index
    };
}
