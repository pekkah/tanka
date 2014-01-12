namespace Web.Bundles
{
    using Infrastructure;
    using Tanka.Nancy.Optimization;

    public class ThemeStyleBundle : StyleBundle
    {
        public ThemeStyleBundle() : base("/css/theme.css")
        {
            var theme = Config.GetValue("tanka/theme");
            Include(theme);
        }
    }
}