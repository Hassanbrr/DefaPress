using DefaPress.Infrastructure.Modules.Base.Interfaces;

namespace DefaPress.Infrastructure.Modules.Tag.Interfaces
{
    public interface ITagRepository : IRepository<Domain.Tag>
    {
        Task<Domain.Tag?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}