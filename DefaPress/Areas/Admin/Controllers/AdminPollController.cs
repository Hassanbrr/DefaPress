using DefaPress.Application.DTOs;
using DefaPress.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace DefaPress.Presentation.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminPollController : Controller
    {
        private readonly IPollService _pollService;

        public AdminPollController(IPollService pollService)
        {
            _pollService = pollService;
        }

        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var polls = await _pollService.GetAllPollsAsync(ct);
            return View(polls);
        }

        public IActionResult Create()
        {
            return PartialView("Create"); // تغییر از "_Create" به "Create"
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePollDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "اطلاعات وارد شده معتبر نیست" });
            }

            try
            {
                var pollId = await _pollService.CreatePollAsync(dto, ct);
                return Json(new { success = true, message = "نظرسنجی با موفقیت ایجاد شد", id = pollId });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Results(int id, CancellationToken ct = default)
        {
            var results = await _pollService.GetPollResultsAsync(id, ct);
            if (results == null)
            {
                return NotFound();
            }
            return PartialView("Results", results); // تغییر از "_Results" به "Results"
        }

        [HttpPut]
        public async Task<IActionResult> Toggle(int id, CancellationToken ct = default)
        {
            try
            {
                var result = await _pollService.TogglePollStatusAsync(id, ct);
                return Json(new { success = result, message = result ? "وضعیت نظرسنجی تغییر کرد" : "خطا در تغییر وضعیت" });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
 
    }
}