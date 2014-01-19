namespace Tanka.Web.Bundles
{
    using Infrastructure;
    using Nancy.Optimization;

    public class ThemeStyleBundle : StyleBundle
    {
        public ThemeStyleBundle() : base("/css/theme.css")
        {
            string theme = Config.GetValue("tanka/theme");
            string hljsTheme = Config.GetValue("tanka/hljs-theme");

            Include(theme);
            Include(hljsTheme);
        }
    }
}