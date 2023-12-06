using ApiApplicationProject.Models;

namespace ApiApplicationProject.Services
{
    public interface IAuthServiece
    {
        Task<AuthModel> RegestrAsync(RegesterModel model);
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
        Task<string> AddRoleAsync(AddRoleModel model);
    }
}
