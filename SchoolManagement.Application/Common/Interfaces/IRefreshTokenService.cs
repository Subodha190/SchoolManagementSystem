using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Application.Common.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> CreateRefreshTokenAsync(ApplicationUser user, string ipAddress);
        Task<RefreshToken> RotateRefreshTokenAsync(RefreshToken token, string ipAddress);
        Task RevokeRefreshTokenAsync(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null);
        Task<RefreshToken> GetRefreshTokenAsync(string token);
    }
}
