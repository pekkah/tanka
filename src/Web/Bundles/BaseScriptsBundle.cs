namespace Web.Bundles
{
    using Tanka.Nancy.Optimization;

    public class BaseScriptsBundle : ScriptBundle
    {
        public BaseScriptsBundle() : base("/js/base.js")
        {
            Include("/Scripts/vendor/highlight.pack.js");
        }
    }
}