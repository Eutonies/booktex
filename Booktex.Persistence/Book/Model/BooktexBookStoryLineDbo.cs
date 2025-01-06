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
internal class BooktexBookStoryLineDbo
{
    public const string TableName = "book_story_line";
    [Column("story_line_id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long StoryLineId { get; set; }
    [Column("story_line")]
    public string StoryLine { get; set; }
    [Column("story_line_name")]
    public string StoryLineName { get; set; }
    [Column("story_line_image")]
    public string StoryLineImage { get; set; }
    [Column("story_line_video")]
    public string StoryLineVideo { get; set; }
    [Column("story_line_font")]
    public string StoryLineFont { get; set; }
    [Column("story_line_att_text")]
    public string StoryLineAttributionText { get; set; }
    [Column("story_line_att_link")]
    public string StoryLineAttributionLink { get; set; }


    public BookStoryLine ToDomain() => new BookStoryLine(
        StoryLine: StoryLine,
        StoryLineName: StoryLineName,
        Image: StoryLineImage,
        Video: StoryLineVideo,
        SiteFont: StoryLineFont,
        Attribution: new BookStoryLineAttribution(Text: StoryLineAttributionText, Link: StoryLineAttributionLink)
    );

}

internal static class BooktexBookStoryLineDboExtensions
{
    public static BooktexBookStoryLineDbo ToDbo(this BookStoryLine lin) => new BooktexBookStoryLineDbo
    {
        StoryLineId = 0L,
        StoryLine = lin.StoryLine,
        StoryLineName = lin.StoryLineName,
        StoryLineImage = lin.Image,
        StoryLineVideo = lin.Video,
        StoryLineFont = lin.SiteFont,
        StoryLineAttributionText = lin.Attribution.Text,
        StoryLineAttributionLink = lin.Attribution.Link
    };
}

