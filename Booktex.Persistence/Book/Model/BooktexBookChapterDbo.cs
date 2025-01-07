using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Persistence.Book.Model;

[Table(TableName)]
internal class BooktexBookChapterDbo
{
    public const string TableName = "book_chapter";

    [Column("chapter_id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ChapterId { get; set; }

    [Column("release_id")]
    public long ReleaseId { get; set; }

    [Column("chapter_date")]
    public DateTime ChapterDate { get; set; }

    [Column("chapter_name")]
    public string ChapterName { get; set; }

    [Column("chapter_order")]
    public string ChapterOrder { get; set; }





}
