using System.ComponentModel.DataAnnotations;

namespace TokenBasedAuthApi.Dto
{
    public class UserRequestDto
    {
        [Required, MinLength(4)] 
        public required string UserName { get; set; }
        [Required, MinLength(8)]
        public required string Password { get; set; }
        [Required, RegularExpression("^(.+)@(.+)$")]
        public required string Email { get; set; }
    }
}
