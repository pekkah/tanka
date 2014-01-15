namespace Tanka.Web.Infrastructure
{
    using System;
    using System.Linq;
    using global::Nancy;
    using global::Nancy.Responses;

    public static class AppHarborCompatibilityExtensions
    {
        public static void RequiresHttpsOrXProto(this INancyModule module, bool redirect = true)
        {
            module.Before.AddItemToEndOfPipeline(RequiresHttpsOrXForwardedProto(redirect));
        }

        private static Func<NancyContext, Response> RequiresHttpsOrXForwardedProto(bool redirect)
        {
            return ctx =>
            {
                Response response = null;
                Request request = ctx.Request;

                string scheme = request.Headers["X-Forwarded-Proto"].SingleOrDefault();

                if (!IsSecure(scheme, request.Url))
                {
                    if (redirect && request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                    {
                        Url redirectUrl = request.Url.Clone();
                        redirectUrl.Scheme = "https";
                        response = new RedirectResponse(redirectUrl.ToString());
                    }
                    else
                    {
                        response = new Response {StatusCode = HttpStatusCode.Forbidden};
                    }
                }

                return response;
            };
        }

        private static bool IsSecure(string scheme, Url url)
        {
            if (scheme == "https")
                return true;

            return url.IsSecure;
        }
    }
}