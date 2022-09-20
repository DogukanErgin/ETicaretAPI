using ETicaretAPI.Application.DTOs.Authentication.InternalAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services.Authentications
{
    public interface IInternalAuthentication
    {
        Task<LoginResponse> LoginAsync(Login model);

        Task<DTOs.Token> RefreshTokenLoginAsync(string refreshToken);
    }
}
