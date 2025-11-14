// Areas/Client/Controllers/PollController.cs
using DefaPress.Application.DTOs;
using DefaPress.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace DefaPress.Presentation.Web.Areas.Client.Controllers
{
    [Area("Client")]
    public class PollController : Controller
    {
        private readonly IPollService _pollService;

        public PollController(IPollService pollService)
        {
            _pollService = pollService;
        }

        [HttpPost("Vote")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vote([FromBody] VoteRequestDto voteRequest, CancellationToken ct = default)
        {
            try
            {
                // گرفتن IP کاربر
                var voterIdentifier = HttpContext.Connection.RemoteIpAddress?.ToString();

                // ثبت رأی
                var result = await _pollService.SubmitVoteAsync(
                    voteRequest.PollId,
                    voteRequest.OptionId,
                    voterIdentifier,
                    ct);

                if (result)
                {
                    return Json(new { success = true, message = "رأی شما با موفقیت ثبت شد" });
                }
                else
                {
                    return Json(new { success = false, message = "امکان ثبت رأی وجود ندارد. ممکن است قبلاً رأی داده باشید." });
                }
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = "خطا در ثبت رأی" });
            }
        }

        [HttpGet("GetResults")]
        public async Task<IActionResult> GetResults(int pollId, CancellationToken ct = default)
        {
            var results = await _pollService.GetPollResultsAsync(pollId, ct);
            return Json(results);
        }
    }
}