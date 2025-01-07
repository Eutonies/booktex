using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Persistence.Book.Model;
[Table(TableName)]
internal class BooktexBookReleaseCharacterDbo
{
    public const string TableName = "book_release_character";

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("release_character_id")]
    public long ReleaseCharacterId { get; set; }

    [Column("release_id")]
    public long ReleaseId { get; set; }

    [Column("character_id")]
    public long CharacterId { get; set; }

    [Column("character_name")]
    public string CharacterName { get; set; }


}
