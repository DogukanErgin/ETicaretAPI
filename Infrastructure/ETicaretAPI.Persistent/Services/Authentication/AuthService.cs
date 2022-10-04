using ETicaretAPI.Application.Abstractions.Services.Authentication;
using ETicaretAPI.Application.Abstractions.Services.User;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Authentication.ExternalAuthentication;
using ETicaretAPI.Application.DTOs.Authentication.InternalAuthentication;
using ETicaretAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services.Authentication
{
    public class AuthService : IAuthService
    {

        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly IConfiguration _configuration;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        readonly IUserService _userService;

        int accessTokenLifeTime = 20;
        int refreshTokenLifeTime = 7;

        public AuthService(ITokenHandler tokenHandler, UserManager<Domain.Entities.Identity.AppUser> userManager, IConfiguration configuration, SignInManager<Domain.Entities.Identity.AppUser> signInManager, IUserService userService)
        {
            _tokenHandler = tokenHandler;
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _userService = userService;
        }

        public async Task<GoogleLoginResponse> GoogleLoginAsync(GoogleLogin model)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["LoginSettings:Google:Client_ID"] }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken, settings);

            var info = new UserLoginInfo(model.Provider, payload.Subject, model.Provider);

            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = payload.Email,
                        UserName = payload.Email,
                        NameSurname = payload.Name
                    };
                    var identityResult = await _userManager.CreateAsync(user);

                    result = identityResult.Succeeded;
                }
            }
            if (result)
                await _userManager.AddLoginAsync(user, info);
            else

                throw new Exception("Geçersiz yetkilendirme");


            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime,user);
            await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, refreshTokenLifeTime);
            return new()
            {
                Token = token
            };
        }

        public async Task<LoginResponse> LoginAsync(Login model)
        {
            {
                Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(model.UserNameOrEmail);

                if (user == null)
                    user = await _userManager.FindByEmailAsync(model.UserNameOrEmail);

                if (user == null)
                    return null;

                SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded) //Authentication başarılı!
                {

                    Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
                    await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, refreshTokenLifeTime);
                    return new()
                    {
                        token = token
                    };
                }
                throw new Exception();
            }

        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {

                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime,user);

                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, refreshTokenLifeTime);

                return token;
            }
            else
                throw new Exception();
        }
    }
}
