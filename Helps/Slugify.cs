using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helps
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class SlugHelper
    {
        public static string SlugifyPersian(string input, int maxLength = 80)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Guid.NewGuid().ToString("n").Substring(0, 8);

            // 1️⃣ نرمال‌سازی یونیکد
            var normalized = input.Normalize(NormalizationForm.FormD);

            // 2️⃣ حذف علائم ترکیبی (مثل اعراب عربی)
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }

            var cleaned = sb.ToString().Normalize(NormalizationForm.FormC);

            // 3️⃣ کوچک‌سازی
            cleaned = cleaned.ToLowerInvariant();

            // 4️⃣ حذف کاراکترهای غیر مجاز (فقط حروف، اعداد و -)
            string slug = Regex.Replace(cleaned, @"[^\p{L}\p{Nd}]+", "-");

            // 5️⃣ حذف - اضافی
            slug = Regex.Replace(slug, "-{2,}", "-").Trim('-');

            // 6️⃣ محدود کردن طول
            if (slug.Length > maxLength - 3) // جا برای "-xy" نگه داریم
                slug = slug.Substring(0, maxLength - 3).Trim('-');

            // 7️⃣ اگر خالی ماند
            if (string.IsNullOrWhiteSpace(slug))
                slug = Guid.NewGuid().ToString("n").Substring(0, 8);

            // 8️⃣ اضافه کردن دو حرف تصادفی انگلیسی
            slug += "-" + GetRandomLetters(2);

            return slug;
        }

        private static string GetRandomLetters(int count)
        {
            const string chars = "ابپتثجچحخدذرزسشصضطظعغفقکگلمنوهی";
            var random = new Random();
            var result = new char[count];
            for (int i = 0; i < count; i++)
                result[i] = chars[random.Next(chars.Length)];
            return new string(result);
        }
    }

}
