using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booktex.Domain.Subscription;
using Booktex.Domain.Util;

namespace Booktex.Persistence.Subscription.Model;
[Table(TableName)]
internal class BooktexSubscriptionExecutionFileDbo
{
    public const string TableName = "subscription_execution_file";

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("exec_file_id")]
    public long FileId { get; set; }

    [Column("execution_id")]
    public long ExecutionId { get; set; }

    [Column("absolute_file_name")]
    public string AbsoluteFileName { get; set; }

    [Column("file_content")]
    public byte[] FileContent { get; set; }


    public BooktexSubscriptionExecutionFile ToDomain() => new BooktexSubscriptionExecutionFile(
        FileId: FileId,
        ExecutionId: ExecutionId,
        AbsoluteFileName: AbsoluteFileName,
        FileContent: FileContent.DeCompress()
        );
}


internal static class BooktexSubscriptionExecutionFileDboExtensions
{
    public static BooktexSubscriptionExecutionFileDbo ToDbo(this BooktexSubscriptionExecutionFile fil) => new BooktexSubscriptionExecutionFileDbo
    {
        FileId = fil.FileId,
        ExecutionId = fil.ExecutionId,
        AbsoluteFileName = fil.AbsoluteFileName,
        FileContent = fil.FileContent.Compress()
    };
}
