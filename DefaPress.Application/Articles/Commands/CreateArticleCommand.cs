 

using MediatR;

namespace DefaPress.Application.Articles.Commands
{
    public record CreateArticleCommand(
        string Title,
        string Summary,
        string Content,
        int ArticleCategoryId,
        IEnumerable<string> Tags,
        string AuthorId,
        DateTime? ScheduledPublishAt = null
    ) : IRequest<int>; // returns created Article Id
}
