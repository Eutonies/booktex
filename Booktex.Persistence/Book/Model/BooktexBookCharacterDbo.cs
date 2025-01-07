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
internal class BooktexBookCharacterDbo
{
    public const string TableName = "book_character";
    [Column("character_id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long CharacterId { get; set; }
    [Column("character_name")]
    public string CharacterName { get; set; }
    [Column("info_character")]
    public string? InfoCharacter { get; set; }
    [Column("info_showname")]
    public string? InfoShowname { get; set; }
    [Column("info_color")]
    public string? InfoColor { get; set; }
    [Column("info_font")]
    public string? InfoFont { get; set; }
    [Column("info_sitefont")]
    public string? InfoSiteFont { get; set; }
    [Column("character_alias")]
    public string? Alias { get; set; }

    public BookCharacter ToDomain() => new BookCharacter(
        CharacterName: CharacterName,
        CharacterInfo: InfoCharacter?.Pipe(
            _ => new BookCharacterInfo(
                Character: InfoCharacter!,
                ShowName: InfoShowname!, 
                Color: InfoColor!,
                Font: InfoFont!,
                SiteFont: InfoSiteFont
            )),
        Alias: Alias
    );

}

internal static class BooktexBookCharacterDboExtensions
{
    public static BooktexBookCharacterDbo ToDbo(this BookCharacter ch) => new BooktexBookCharacterDbo
    {
        CharacterName = ch.CharacterName,
        InfoCharacter = ch.CharacterInfo?.Character,
        InfoShowname = ch.CharacterInfo?.ShowName,
        InfoColor = ch.CharacterInfo?.Color,
        InfoFont = ch.CharacterInfo?.Font,
        InfoSiteFont = ch.CharacterInfo?.SiteFont,
        Alias = ch.Alias
    };
}
