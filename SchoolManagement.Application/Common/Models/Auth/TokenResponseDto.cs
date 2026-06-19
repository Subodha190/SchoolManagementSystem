namespace SchoolManagement.Application.Common.Models.Auth
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }
    }
}
