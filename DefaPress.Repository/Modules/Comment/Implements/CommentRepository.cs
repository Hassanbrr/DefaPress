using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Base.Implements;
using DefaPress.Infrastructure.Modules.Comment.Interfaces;

namespace DefaPress.Infrastructure.Modules.Comment.Implements
{
    public class CommentRepository : Repository<Domain.Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}