using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Persistence.Book.Model;

[Table(TableName)]
internal class BooktexBookReleaseDbo
{
    public const string TableName = "book_release";
    [Column("release_id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ReleaseId { get; set; }

    [Column("release_author")]
    public string Author { get; set; }

    [Column("release_version")]
    public string Version { get; set; }

    [Column("last_modified")]
    public DateTime LastModified { get; set; }




}
