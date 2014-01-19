namespace Tanka.Web.Bundles
{
    using Nancy.Optimization;

    public class HeadScriptsBundle : ScriptBundle
    {
        public HeadScriptsBundle() : base("/js/head.js")
        {
            Include("/Scripts/jquery-2.0.3.min.js");
            Include("/Scripts/modernizr-2.7.1.js");
            Include("/Scripts/moment-with-langs.js");
        }
    }
}