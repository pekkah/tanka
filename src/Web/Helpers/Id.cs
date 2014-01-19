namespace Tanka.Web.Helpers
{
    using System;

    public static class Id
    {
        public static int WithoutCollection(string idWithCollection)
        {
            string idString = idWithCollection.Substring(idWithCollection.LastIndexOf('/') + 1);

            return Convert.ToInt32(idString);
        }
    }
}