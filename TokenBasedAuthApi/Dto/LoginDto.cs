using System.ComponentModel.DataAnnotations;

namespace TokenBasedAuthApi.Dto
{
    public class LoginDto
    {
        [Required, MinLength(4)]
        public required string UserName { get; set; }
        [Required, MinLength(4)] 
        public required string Password { get; set; }
    }
}
