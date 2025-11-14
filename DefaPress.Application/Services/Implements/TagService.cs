using AutoMapper;
using DefaPress.Application.Services.Interfaces;
using DefaPress.Domain;
using DefaPress.Infrastructure.Modules.Base.Interfaces;

namespace DefaPress.Application.Services.Implements
{
    public class TagService : ITagService
    {
        private readonly IUnitOffWork _unitOffWork;
        private readonly IMapper _mapper;

        public TagService(IUnitOffWork unitOffWork, IMapper mapper)
        {
            _unitOffWork = unitOffWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOffWork.TagRepository.GetAllAsync();
        }

        public async Task<Tag?> GetTagByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _unitOffWork.TagRepository.GetByIdAsync(id, null, cancellationToken);
        }

        public async Task<IEnumerable<Tag>> SearchTagsAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllTagsAsync(cancellationToken);

            var allTags = await GetAllTagsAsync(cancellationToken);
            return allTags.Where(t => t.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Tag> CreateTagAsync(string name, CancellationToken cancellationToken = default)
        {
            var tag = new Tag { Name = name };
            await _unitOffWork.TagRepository.AddAsync(tag, cancellationToken);
            await _unitOffWork.SaveChangesAsync(cancellationToken);
            return tag;
        }

        public async Task<bool> DeleteTagAsync(int id, CancellationToken cancellationToken = default)
        {
            var tag = await _unitOffWork.TagRepository.GetByIdAsync(id, null, cancellationToken);
            if (tag == null) return false;

            _unitOffWork.TagRepository.Remove(tag, cancellationToken);
            await _unitOffWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}