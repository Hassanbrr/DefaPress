using DefaPress.Domain;

namespace DefaPress.Application.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync(CancellationToken cancellationToken = default);
        Task<Tag?> GetTagByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Tag>> SearchTagsAsync(string searchTerm, CancellationToken cancellationToken = default);
        Task<Tag> CreateTagAsync(string name, CancellationToken cancellationToken = default);
        Task<bool> DeleteTagAsync(int id, CancellationToken cancellationToken = default);
    }
}