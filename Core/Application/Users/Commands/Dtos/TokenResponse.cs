namespace CleanArchitecture.Application.Users.Commands.Dtos
{
    public record TokenResponse
    {
        public string Token { get; init; }
        public string RefreshToken { get; init; }
        public DateTime Expiration { get; init; }

        public TokenResponse(string token, string refreshToken, DateTime expiration)
        {
            Token = token;
            RefreshToken = refreshToken;
            Expiration = expiration;
        }
    }
}
