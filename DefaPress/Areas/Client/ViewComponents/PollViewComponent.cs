 
using DefaPress.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DefaPress.Presentation.Web.Areas.Client.ViewComponents
{
    public class PollViewComponent : ViewComponent
    {
        private readonly IPollService _pollService;

        public PollViewComponent(IPollService pollService)
        {
            _pollService = pollService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // گرفتن IP کاربر برای شناسایی
            var voterIdentifier = HttpContext.Connection.RemoteIpAddress?.ToString();

            // دریافت نظرسنجی فعال
            var poll = await _pollService.GetActivePollAsync(voterIdentifier);

            // اگر نظرسنجی فعال وجود نداشت، چیزی نمایش نده
            if (poll == null)
                return Content(string.Empty);

            return View(poll);
        }
    }
}