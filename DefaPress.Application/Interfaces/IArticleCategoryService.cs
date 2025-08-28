using DefaPress.Application.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DefaPress.Application.Interfaces
{
    public interface IArticleCategoryService
    {
        /// <summary>
        /// بازگشت همهٔ دسته‌ها به صورت مسطح (flat list).
        /// </summary>
        Task<IEnumerable<ArticleCategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// بازگشت درخت دسته‌ها (فقط ریشه‌ها با زیردسته‌ها تا عمق مشخص).
        /// </summary>
        Task<IEnumerable<ArticleCategoryDto>> GetCategoryTreeAsync(int maxDepth = 5, CancellationToken cancellationToken = default);

        /// <summary>
        /// بازگشت یک دسته بر اساس id. در صورت عدم وجود، null برمی‌گرداند.
        /// </summary>
        Task<ArticleCategoryDto?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// اضافه‌کردن یک دسته و بازگرداندن شناسهٔ ساخته‌شده.
        /// </summary>
        Task<int> AddCategoryAsync(ArticleCategoryCreateDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// بروزرسانی یک دسته. اگر موفق بود، true برمی‌گرداند؛ در غیر این صورت false.
        /// </summary>
        Task<bool> UpdateCategoryAsync(int id, ArticleCategoryCreateDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// حذف یک دسته. اگر موفق بود true برمی‌گرداند.
        /// (اگر دسته زیردسته داشته باشد یا مقالات به آن وصل باشند ممکن است حذف ناموفق باشد)
        /// </summary>
        Task<bool> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// تغییر والد یک دسته (move) — بررسی می‌کند حلقه ایجاد نشود.
        /// </summary>
        Task<bool> MoveCategoryAsync(int id, int? newParentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// بروزرسانی‌های مرتب‌سازی (display order) برای چند دسته.
        /// نمونه ورودی: [(categoryId, displayOrder), ...]
        /// </summary>
        Task UpdateDisplayOrdersAsync(IEnumerable<(int CategoryId, int DisplayOrder)> orders, CancellationToken cancellationToken = default);
    }
}
