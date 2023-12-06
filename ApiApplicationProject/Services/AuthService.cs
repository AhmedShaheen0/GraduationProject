using ApiApplicationProject.Constes;
using ApiApplicationProject.Helpers;
using ApiApplicationProject.Models;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiApplicationProject.Services
{
    public class AuthService : IAuthServiece
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
        }

        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);


            if (user is null || !await _roleManager.RoleExistsAsync(model.role))
                return "Invalid User ID or Role";

            if (await _userManager.IsInRoleAsync(user, model.role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.role);

            return result.Succeeded? string.Empty : "Something went wrong!";
        }

        public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
        {
            var authmodel = new AuthModel();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authmodel.Message = "Email or Password is incorrect";
                return authmodel;
            }


            var jwtsecuritytoken = await CreatejwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authmodel.IsAuthenticated = true;
            authmodel.Token = new JwtSecurityTokenHandler().WriteToken(jwtsecuritytoken);
            authmodel.Email = user.Email;
            authmodel.UserName = user.UserName;
            authmodel.ExpireOn = jwtsecuritytoken.ValidTo;
            authmodel.Roles = rolesList.ToList();

            return authmodel;
        }

        public async Task<AuthModel> RegestrAsync(RegesterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email is already registered!" };


            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthModel { Message = "UserName is already registered!" };

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Birthday = model.Birthday
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new AuthModel { Message = errors };

            }

            await _userManager.AddToRoleAsync(user, AppRoles.User);
            var jwtsecuritytoken = await CreatejwtToken(user);
            return new AuthModel
            {
                Email = user.Email,
                ExpireOn = jwtsecuritytoken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { AppRoles.User },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtsecuritytoken),
                UserName = user.UserName,
            };
        }
        private async Task<JwtSecurityToken> CreatejwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaim = new List<Claim>();

            foreach (var role in roles)
                roleClaim.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid",user.Id)
             }
            .Union(userClaims)
            .Union(roleClaim);

            var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key!));
            var SigningCredentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: SigningCredentials
                );

            return jwtSecurityToken;

        }
    }

}
