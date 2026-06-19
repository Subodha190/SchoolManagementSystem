using Microsoft.EntityFrameworkCore;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Persistence;
using System;

namespace SchoolManagement.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> CreateRefreshTokenAsync(ApplicationUser user, string ipAddress)
        {
            var refresh = new RefreshToken
            {
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Expires = DateTime.UtcNow.AddDays(7),
                CreatedByIp = ipAddress,
                ApplicationUserId = user.Id
            };

            _context.Add(refresh);
            await _context.SaveChangesAsync();
            return refresh;
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
            return await _context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task<RefreshToken> RotateRefreshTokenAsync(RefreshToken token, string ipAddress)
        {
            var newToken = new RefreshToken
            {
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Expires = DateTime.UtcNow.AddDays(7),
                CreatedByIp = ipAddress,
                ApplicationUserId = token.ApplicationUserId
            };

            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReplacedByToken = newToken.Token;

            _context.Add(newToken);
            await _context.SaveChangesAsync();

            return newToken;
        }

        public async Task RevokeRefreshTokenAsync(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReplacedByToken = replacedByToken;
            await _context.SaveChangesAsync();
        }
    }
}
