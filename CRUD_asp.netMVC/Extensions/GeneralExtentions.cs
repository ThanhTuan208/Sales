using System.Globalization;
using System.Text;

namespace CRUD_asp.netMVC.Extensions
{
    public  static class GeneralExtentions
    {
        // Kiem tra chuoi co dau
        public static bool HasDiacritics(string text)
        {
            string removeDiacritics = RemoveDiacritics(text) ?? string.Empty;
            return text.Equals(removeDiacritics) ? true : false;
        }

        // Thay doi name co cac ki tu co dau thanh khong dau
        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var stringNormal = text.Normalize(NormalizationForm.FormD);
            StringBuilder builder = new StringBuilder();

            foreach (char c in stringNormal)
            {
                var unicodeCate = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCate != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC).Replace(" ", "");
        }
    }
}
