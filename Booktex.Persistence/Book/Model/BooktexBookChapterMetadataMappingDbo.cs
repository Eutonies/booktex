using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Persistence.Book.Model;
[Table(TableName)]
internal class BooktexBookChapterMetadataMappingDbo
{
    public const string TableName = "book_chapter_metadata_mapping";
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("metadata_mapping_id")]
    public long MappingId { get; set; }

    [Column("metadata_id")]
    public long MatadataId { get; set; }

    [Column("map_from")]
    public string MapFrom { get; set; }

    [Column("map_to")]
    public string MapTo { get; set; }


}
