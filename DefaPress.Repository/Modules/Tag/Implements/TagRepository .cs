using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Base.Implements;
using DefaPress.Infrastructure.Modules.Tag.Interfaces;

namespace DefaPress.Infrastructure.Modules.Tag.Implements
{
    public class TagRepository : Repository<Domain.Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}