namespace Tanka.Web.Models
{
    using System.Collections.Generic;

    public class UsersDto
    {
        public IEnumerable<UserDto> Users { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }
}