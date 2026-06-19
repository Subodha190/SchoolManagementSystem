using Microsoft.AspNetCore.Identity;
using SchoolManagement.Application.Common.Interfaces;
using SchoolManagement.Application.Common.Models.Auth;
using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthService(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService, IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<TokenResponseDto> RegisterAsync(RegisterUserDto request, string ipAddress)
        {
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                FullName = request.FullName,
                SchoolId = request.SchoolId,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(",", result.Errors.Select(x => x.Description)));
            }

            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                await _userManager.AddToRoleAsync(user, request.Role);
            }

            var accessToken = await _jwtTokenService.GenerateTokenAsync(user);
            var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user, ipAddress);

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresIn = (int)TimeSpan.FromMinutes(120).TotalSeconds
            };
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new Exception("Invalid Email");

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
                throw new Exception("Invalid Password");

            var accessToken = await _jwtTokenService.GenerateTokenAsync(user);

            var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user, ipAddress);

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresIn = (int)TimeSpan.FromMinutes(120).TotalSeconds
            };
        }

        public async Task<TokenResponseDto> RefreshAsync(string token, string ipAddress)
        {
            var existing = await _refreshTokenService.GetRefreshTokenAsync(token);
            if (existing == null || !existing.IsActive)
                throw new Exception("Invalid refresh token");

            var user = await _userManager.FindByIdAsync(existing.ApplicationUserId.ToString());
            if (user == null)
                throw new Exception("Invalid token user");

            // rotate
            var newRefresh = await _refreshTokenService.RotateRefreshTokenAsync(existing, ipAddress);

            var accessToken = await _jwtTokenService.GenerateTokenAsync(user);

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefresh.Token,
                ExpiresIn = (int)TimeSpan.FromMinutes(120).TotalSeconds
            };
        }

        public async Task RevokeAsync(string token, string ipAddress)
        {
            var existing = await _refreshTokenService.GetRefreshTokenAsync(token);
            if (existing == null || !existing.IsActive)
                throw new Exception("Invalid refresh token");

            await _refreshTokenService.RevokeRefreshTokenAsync(existing, ipAddress);
        }
    }
}
