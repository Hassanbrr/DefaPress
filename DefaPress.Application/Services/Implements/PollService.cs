using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Application.Services.Interfaces;
using DefaPress.Domain.PollModels;
using DefaPress.Infrastructure.Modules.Base.Interfaces;

namespace DefaPress.Application.Services.Implements
{
    public class PollService : IPollService
    {
        private readonly IUnitOffWork _unitOfWork;
        private readonly IMapper _mapper;

        public PollService(IUnitOffWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PollDto> GetActivePollAsync(string voterIdentifier = null, CancellationToken ct = default)
        {
            var activePolls = await _unitOfWork.PollRepository.FindAsync(
                p => p.IsActive && (p.ExpiresAt == null || p.ExpiresAt > DateTime.Now),
                "Options,Options.Votes",
                ct);

            var poll = activePolls.OrderByDescending(p => p.CreatedAt).FirstOrDefault();

            if (poll == null) return null;

            return await MapPollToDto(poll, voterIdentifier, ct);
        }

        public async Task<bool> SubmitVoteAsync(int pollId, int optionId, string voterIdentifier, CancellationToken ct = default)
        {
            // بررسی وجود نظرسنجی
            var poll = await _unitOfWork.PollRepository.GetByIdAsync(pollId, ct: ct);
            if (poll == null || !poll.IsActive) return false;

            // بررسی وجود گزینه
            var option = await _unitOfWork.PollOptionRepository.GetByIdAsync(optionId, ct: ct);
            if (option == null || option.PollId != pollId) return false;

            // بررسی اینکه کاربر قبلاً رأی نداده باشد
            var hasVoted = await _unitOfWork.PollVoteRepository.AnyAsync(
                v => v.PollId == pollId &&
                     (v.VoterIp == voterIdentifier || v.VoterUserId == voterIdentifier),
                ct);

            if (hasVoted) return false;

            // ثبت رأی جدید
            var vote = new PollVote
            {
                PollId = pollId,
                OptionId = optionId,
                VoterIp = voterIdentifier,
                VotedAt = DateTime.Now
            };

            await _unitOfWork.PollVoteRepository.AddAsync(vote, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }

        public async Task<int> CreatePollAsync(CreatePollDto dto, CancellationToken ct = default)
        {
            // غیرفعال کردن نظرسنجی‌های قبلی
            var activePolls = await _unitOfWork.PollRepository.FindAsync(p => p.IsActive, ct: ct);
            foreach (var activePoll in activePolls)
            {
                activePoll.IsActive = false;
                _unitOfWork.PollRepository.Update(activePoll);
            }

            // ایجاد نظرسنجی جدید
            var poll = new Poll
            {
                Question = dto.Question,
                IsActive = true,
                CreatedAt = DateTime.Now,
                ExpiresAt = dto.DurationDays.HasValue ? DateTime.Now.AddDays(dto.DurationDays.Value) : null
            };

            await _unitOfWork.PollRepository.AddAsync(poll, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            // ایجاد گزینه‌ها
            for (int i = 0; i < dto.Options.Count; i++)
            {
                var option = new PollOption
                {
                    PollId = poll.Id,
                    Text = dto.Options[i],
                    Order = i
                };
                await _unitOfWork.PollOptionRepository.AddAsync(option, ct);
            }

            await _unitOfWork.SaveChangesAsync(ct);
            return poll.Id;
        }

        public async Task<PollDto> GetPollResultsAsync(int pollId, CancellationToken ct = default)
        {
            var poll = await _unitOfWork.PollRepository.GetByIdAsync(pollId, "Options,Options.Votes", ct);
            if (poll == null) return null;

            return await MapPollToDto(poll, null, ct);
        }

        public async Task<List<PollDto>> GetAllPollsAsync(CancellationToken ct = default)
        {
            var polls = await _unitOfWork.PollRepository.GetAllAsync("Options,Options.Votes", ct);
            var pollDtos = new List<PollDto>();

            foreach (var poll in polls.OrderByDescending(p => p.CreatedAt))
            {
                var pollDto = await MapPollToDto(poll, null, ct);
                pollDtos.Add(pollDto);
            }

            return pollDtos;
        }

        public async Task<bool> TogglePollStatusAsync(int pollId, CancellationToken ct = default)
        {
            var poll = await _unitOfWork.PollRepository.GetByIdAsync(pollId, ct: ct);
            if (poll == null) return false;

            poll.IsActive = !poll.IsActive;
            _unitOfWork.PollRepository.Update(poll);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }

        private async Task<PollDto> MapPollToDto(Poll poll, string voterIdentifier, CancellationToken ct)
        {
            var totalVotes = poll.Votes?.Count ?? 0;
            var hasVoted = false;

            if (!string.IsNullOrEmpty(voterIdentifier))
            {
                hasVoted = await _unitOfWork.PollVoteRepository.AnyAsync(
                    v => v.PollId == poll.Id &&
                         (v.VoterIp == voterIdentifier || v.VoterUserId == voterIdentifier),
                    ct);
            }

            var pollDto = _mapper.Map<PollDto>(poll);
            pollDto.HasVoted = hasVoted;
            pollDto.TotalVotes = totalVotes;

            // محاسبه درصدها
            foreach (var optionDto in pollDto.Options)
            {
                var option = poll.Options.First(o => o.Id == optionDto.Id);
                var optionVoteCount = option.Votes?.Count ?? 0;
                optionDto.VoteCount = optionVoteCount;
                optionDto.Percentage = totalVotes > 0 ?
                    Math.Round((optionVoteCount * 100.0) / totalVotes, 1) : 0;
            }

            return pollDto;
        }
    }
}