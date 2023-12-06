using ApiApplicationProject.Models;

namespace ApiApplicationProject.Services
{
    public interface IActionService
    {
        IEnumerable<ActionModel> GetEvents();
        ActionModel GetEventById(int id);
        Task<ActionModel> CreateEvent(EvntModel ev, string userId);
    }
}
