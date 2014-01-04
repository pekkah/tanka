namespace Web.Documents
{
    using System;

    public class AuthToken
    {
        public string UserName { get; set; }

        public DateTime Created { get; set; }

        public TimeSpan Lifetime { get; set; }

        public string Token { get; set; }
    }
}