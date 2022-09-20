using ETicaretAPI.Application.Abstractions.Services.User;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.Email,
                NameSurname = model.NameSurname,
            }, model.Password);


            CreateUserResponse response = new()
            {
                Succeeded = result.Succeeded,

            };

            if (result.Succeeded)
                response.Message = "Kullanıcı başarıyla oluşturulmuştur";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description} \n ";

            return response;
        }

        public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate,int refreshTokenLifeTime)
        {


            if (user!=null){
                user.RefreshToken=refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(refreshTokenLifeTime);
                await _userManager.UpdateAsync(user);
            }
            else
            throw new Exception();
        }
    }
}
