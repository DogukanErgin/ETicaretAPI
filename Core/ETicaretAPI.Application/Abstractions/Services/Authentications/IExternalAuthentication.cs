using ETicaretAPI.Application.DTOs.Authentication.ExternalAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services.Authentications
{
    public interface IExternalAuthentication
    {
        Task<GoogleLoginResponse> GoogleLoginAsync(GoogleLogin model);
    }
}
