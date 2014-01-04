namespace Web.Models
{
    using System.Collections.Generic;

    public class CreateUserRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}