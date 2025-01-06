using Booktex.Domain.Subscription;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Persistence.Subscription.Model;
[Table(TableName)]
internal class BooktexSubscriptionExecutionDbo
{
    public const string TableName = "subscription_execution";

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("execution_id")]
    public long ExecutionId { get; set; }

    [Column("subscription_id")]
    public long SubscriptionId { get; set; }

    [Column("execution_time")]
    public DateTime ExecutionTime { get; set; }

    [Column("hash_code")]
    public string HashCode { get; set; }


    public BooktexSubscriptionExecution ToDomain(IEnumerable<BooktexSubscriptionExecutionFileDbo> files) => new BooktexSubscriptionExecution(
        ExecutionId: ExecutionId,
        SubscriptionId: SubscriptionId, 
        ExecutionTime: ExecutionTime,
        Files: files
            .OrderBy(_ => _.AbsoluteFileName)
            .Select(_ => _.ToDomain())
            .ToList()
        );

}

internal static class BooktexSubscriptionExecutionDboExtensions
{
    public static BooktexSubscriptionExecutionDbo ToDbo(this BooktexSubscriptionExecution execution) => new BooktexSubscriptionExecutionDbo
    {
        ExecutionId = execution.ExecutionId,
        SubscriptionId = execution.SubscriptionId,
        ExecutionTime = execution.ExecutionTime,
        HashCode = execution.HashCode
    };
}
