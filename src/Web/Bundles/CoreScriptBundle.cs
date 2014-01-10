namespace Web.Bundles
{
    using Tanka.Nancy.Optimization;

    public class CoreScriptBundle : ScriptBundle
    {
        public CoreScriptBundle() : base("/js/core.js")
        {
            Include("/Scripts/jquery-2.0.3.min.js");
            Include("/Scripts/modernizr-2.7.1.js");
            Include("/Scripts/moment-with-langs.js");
        }
    }

    public class ThemeStyleBundle : StyleBundle
    {
        public ThemeStyleBundle() : base("/css/theme.css")
        {
            // todo: use theme path from 
            Include("/Content/themes/default/bootstrap.css");
        }
    }
}