using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;

        public LoginUserCommandHandler(ITokenHandler tokenHandler, SignInManager<Domain.Entities.Identity.AppUser> signInManager, UserManager<Domain.Entities.Identity.AppUser> userManager)
        {
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
            if(user==null)
               user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            if (user == null)
                return new LoginUserErrorCommandResponse() { 
                Message="Kullanıcı adı veya şifre hatalı"
                };

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(5);
            }
            return new LoginUserErrorCommandResponse()
            {
                Message = "Kimlik doğrulama hatası"
            };
        }
    }
}
