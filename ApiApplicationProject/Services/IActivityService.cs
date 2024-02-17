using ApiApplicationProject.Models;

namespace ApiApplicationProject.Services
{
    public interface IActivityService
    {
        IEnumerable<ActivityModel> GetEvents();
        IEnumerable<ActivityModel> GetLog();
        ActivityModel GetEventById(int id);
        Task<ActivityModel> CreateEvent(ActivityModel ev, string userId);
    }
}
