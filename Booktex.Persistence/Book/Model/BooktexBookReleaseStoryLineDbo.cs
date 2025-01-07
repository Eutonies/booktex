using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Persistence.Book.Model;
[Table(TableName)]
internal class BooktexBookReleaseStoryLineDbo
{
    public const string TableName = "book_release_storyline";

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("release_storyline_id")]
    public long ReleaseStoryLineId { get; set; }

    [Column("release_id")]
    public long ReleaseId { get; set; }

    [Column("storyline_id")]
    public long StoryLineId { get; set; }

    [Column("storyline_name")]
    public string StoryLineName { get; set; }


}
