namespace TokenBasedAuthApi.Dto
{
    public class UserResponseDto
    {
        public UserResponseDto(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }

        public string UserName { get; set; }
        
        public string Email { get; set; }
    }
}
