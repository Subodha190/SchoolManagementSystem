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

        private static string GenerateSecureToken(int size = 64)
        {
            var bytes = new byte[size];
            System.Security.Cryptography.RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));
            return Convert.ToBase64String(bytes);
        }

        public async Task<RefreshToken> CreateRefreshTokenAsync(ApplicationUser user, string ipAddress)
        {
            var rawToken = GenerateSecureToken(64);
            var refresh = new RefreshToken
            {
                TokenHash = ComputeSha256Hash(rawToken),
                Expires = DateTime.UtcNow.AddDays(7),
                CreatedByIp = ipAddress,
                ApplicationUserId = user.Id
                ,
                SchoolId = user.SchoolId
            };

            _context.Add(refresh);
            await _context.SaveChangesAsync();
            // set raw token transiently so caller can receive it (not stored)
            refresh.Token = rawToken;
            await _context.SaveChangesAsync();
            return refresh;
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;
            var hash = ComputeSha256Hash(token);
            var rt = await _context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.TokenHash == hash && (x.SchoolId == 0 || x.SchoolId == _context.CurrentSchoolId));
            if (rt == null) return null;

            // Check expiration
            if (rt.IsExpired) return null;

            return rt;
        }

        public async Task<RefreshToken> RotateRefreshTokenAsync(RefreshToken token, string ipAddress)
        {
            var rawNewToken = GenerateSecureToken(64);
            var newToken = new RefreshToken
            {
                TokenHash = ComputeSha256Hash(rawNewToken),
                Expires = DateTime.UtcNow.AddDays(7),
                CreatedByIp = ipAddress,
                ApplicationUserId = token.ApplicationUserId
                ,
                SchoolId = token.SchoolId
            };

            // Reuse detection: if token is already revoked, deny rotation
            if (token.Revoked != null)
                throw new InvalidOperationException("Refresh token has been revoked");

            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReplacedByToken = newToken.TokenHash;

            newToken.Token = rawNewToken;
            _context.Add(newToken);
            await _context.SaveChangesAsync();

            // return new token with raw token in transient property
            return newToken;
        }

        public async Task RevokeRefreshTokenAsync(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            if (token.Revoked != null)
                return; // already revoked

            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReplacedByToken = replacedByToken;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateAndUseRefreshTokenAsync(string token, string ipAddress)
        {
            var rt = await GetRefreshTokenAsync(token);
            if (rt == null) return false;

            // If revoked or expired - invalid
            if (rt.Revoked != null || rt.IsExpired) return false;

            // Update last used
            rt.LastUsedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
