namespace Tanka.Web.Documents
{
    using System;
    using System.Collections.Generic;

    public class User
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public IEnumerable<string> Roles { get; set; }
        public Guid Identifier { get; set; }
    }
}