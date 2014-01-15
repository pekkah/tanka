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

                

                if (!IsSecure(request))
                {
                    if (redirect && request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                    {
                        Url redirectUrl = request.Url.Clone();
                        redirectUrl.Scheme = "https";
                        redirectUrl.Port = 443;
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

        private static bool IsSecure(Request request)
        {
            if (request.Headers.Keys.Contains("X-Forwarded-Proto"))
            {
                var scheme = request.Headers["X-Forwarded-Proto"].FirstOrDefault();
                
                if (!string.IsNullOrWhiteSpace(scheme) && scheme == "https")
                    return true;
            }

            return request.Url.IsSecure;
        }
    }
}