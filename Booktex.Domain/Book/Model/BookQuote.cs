namespace Booktex.Domain.Book.Model;
public record BookQuote(
    string? Name,
    string? SubName,
    string QuoteString
    ) : BookChapterContent;
