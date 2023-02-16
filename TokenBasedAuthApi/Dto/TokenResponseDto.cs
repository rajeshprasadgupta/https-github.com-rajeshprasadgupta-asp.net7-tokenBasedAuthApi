namespace TokenBasedAuthApi.Dto
{
    public class TokenResponseDto
    {
        public required string AccessToken { get; set; }
        public required DateTime Expiration { get; set; }
    }
}
