using ETicaretAPI.Application.Abstractions.Services.Authentications;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Authentication.InternalAuthentication;
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
        readonly IInternalAuthentication _authService;

        public LoginUserCommandHandler(IInternalAuthentication authService)
        {
            _authService = authService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
           var token = await _authService.LoginAsync(new()
            {
                Password = request.Password,
                UserNameOrEmail = request.UserNameOrEmail,
                
            });

            if (token!=null) {
                return new LoginUserSuccessCommandResponse()
                {

                    Token = token.token
                };
            }
            else
            {
                return new LoginUserErrorCommandResponse()
                {
                    Message = "Kullanıcı adı veya şifre hatalı"
                };
            }
        }
    }
}
