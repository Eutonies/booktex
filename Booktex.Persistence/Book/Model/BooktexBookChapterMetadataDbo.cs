using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Persistence.Book.Model;
[Table(TableName)]
internal class BooktexBookChapterMetadataDbo
{
    public const string TableName = "book_chapter_metadata";
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("metadata_id")]
    public long MetadataId { get; set; }

    [Column("chapter_id")]
    public long ChapterId { get; set; }

    [Column("chapter_title")]
    public string? ChapterTitle { get; set; }

    [Column("chapter_date")]
    public DateTime? ChapterDate { get; set; }

    [Column("story_line_key")]
    public string? StoryLineKey { get; set; }

    [Column("chapter_order")]
    public string? ChapterOrder { get; set; }


    public BookChapterMetaData ToDomain(IEnumerable<BooktexBookChapterMetadataMappingDbo> mappings) => new BookChapterMetaData(
        ChapterTitle: ChapterTitle,
        ChapterDate: ChapterDate,
        StoryLineKey: StoryLineKey,
        Aliases: mappings
            .ToDictionarySafe(_ => _.MapFrom, _ => _.MapTo),
        ChapterOrder: ChapterOrder
    );

    public void UpdateMetadataIds(IEnumerable<BooktexBookChapterMetadataMappingDbo> mappings)
    {
        foreach(var mapp in mappings)
            mapp.MetadataId = MetadataId;
    }

}

internal static class BooktexBookChapterMetadataDboExtensions
{
    public static (BooktexBookChapterMetadataDbo Metadata, IReadOnlyCollection<BooktexBookChapterMetadataMappingDbo> Mappings) ToDbo(this BookChapterMetaData meta) => (
        Metadata: new BooktexBookChapterMetadataDbo
        {
            ChapterTitle = meta.ChapterTitle,
            ChapterDate = meta.ChapterDate,
            StoryLineKey = meta.StoryLineKey,
            ChapterOrder = meta.ChapterOrder
        },
        Mappings: meta.Aliases?
            .Select(_ => new BooktexBookChapterMetadataMappingDbo { MapFrom = _.Key, MapTo = _.Value })?
            .ToList() 
            ?? []
    );
}

