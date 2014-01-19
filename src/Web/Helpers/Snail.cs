namespace Tanka.Web.Helpers
{
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Snail
    {
        public static string ToSlug(string text)
        {
            string normalized = Normalize(text.ToLower().Trim());

            string slugified = Slugify(normalized);

            return slugified;
        }

        private static string Slugify(string normalized, int maxLength = 50)
        {
            string str = normalized;

            // invalid chars, make into spaces
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            // convert multiple spaces/hyphens into one space       
            str = Regex.Replace(str, @"[\s-]+", " ").Trim();

            // cut and trim it
            str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();

            // hyphens
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }

        /// <summary>
        ///     Source : http://blogs.msdn.com/b/michkap/archive/2007/05/14/2629747.aspx
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string Normalize(string text)
        {
            string stFormD = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}