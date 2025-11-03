using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Base.Implements;
using DefaPress.Infrastructure.Modules.MediaFile.Interfaces;

namespace DefaPress.Infrastructure.Modules.MediaFile.Implements
{
    public class MediaFileRepository : Repository<Domain.MediaFile>, IMediaFileRepository
    {
        public MediaFileRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}