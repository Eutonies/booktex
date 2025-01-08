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
internal class BooktexBookAboutTheAuthorDbo
{
    public const string TableName = "book_about_author";

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("about_the_author_id")]
    public long AboutTheAuthorId { get; set; }

    [Column("release_id")]
    public long ReleaseId { get; set; }

}

internal static class BooktexBookAboutTheAuthorDboExtensions
{
    public static BooktexBookAboutTheAuthorDbo ToDbo(this BookAboutTheAuthor ab) => new BooktexBookAboutTheAuthorDbo
    {
    };
}

