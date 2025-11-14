using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Base.Implements;
using DefaPress.Infrastructure.Modules.Tag.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DefaPress.Infrastructure.Modules.Tag.Implements
{
    public class TagRepository : Repository<Domain.Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Domain.Tag?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Domain.Tag>()
                .FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
        }
    }
}