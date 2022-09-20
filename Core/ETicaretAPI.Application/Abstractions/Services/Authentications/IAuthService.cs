using ETicaretAPI.Application.Abstractions.Services.Authentications;
using ETicaretAPI.Application.DTOs.Authentication.ExternalAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services.Authentication
{
    public interface IAuthService : IExternalAuthentication, IInternalAuthentication
    {

    
        
    }
}
