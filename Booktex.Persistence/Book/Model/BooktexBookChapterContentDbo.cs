using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Persistence.Book.Model;
[Table(TableName)]
internal class BooktexBookChapterContentDbo
{
    public const string TableName = "book_chapter_content";
    [Column("content_id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ContentId { get; set; }

    [Column("content_type")]
    public BooktexBookChapterContentTypeDbo ContentType { get; set; }

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

}
