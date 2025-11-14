using DefaPress.Application.DTOs;

namespace DefaPress.Application.Services.Interfaces
{
    public interface IPollService
    {
        Task<PollDto> GetActivePollAsync(string voterIdentifier = null, CancellationToken ct = default);
        Task<bool> SubmitVoteAsync(int pollId, int optionId, string voterIdentifier, CancellationToken ct = default);
        Task<int> CreatePollAsync(CreatePollDto dto, CancellationToken ct = default);
        Task<PollDto> GetPollResultsAsync(int pollId, CancellationToken ct = default);
        Task<List<PollDto>> GetAllPollsAsync(CancellationToken ct = default);
        Task<bool> TogglePollStatusAsync(int pollId, CancellationToken ct = default);
    }
}