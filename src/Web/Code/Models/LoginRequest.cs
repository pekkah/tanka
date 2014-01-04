namespace Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginRequest
    {
        [Required]
        [MinLength(3)]
        public string UserName { get; set; }

        [Required]
        [MinLength(4)]
        public string Password { get; set; }
    }
}