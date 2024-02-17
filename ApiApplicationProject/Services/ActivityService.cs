using ApiApplicationProject.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiApplicationProject.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ActivityService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public async Task<ActivityModel> CreateEvent(ActivityModel ev, string userId)
        {
            

            var model = new ActivityModel
            {
               ApplicationUserId= userId,
             Timestamp= DateTime.UtcNow,
               Title= ev.Title,
               
            };
            _context.Activities.Add(model);
            _context.SaveChanges();
            return model;
        }

        public ActivityModel GetEventById(int id)
        {
         
            
            return _context.Activities.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<ActivityModel> GetEvents()
        {
            return _context.Activities.ToList();
        }

        public IEnumerable<ActivityModel> GetLog()
        {
            return _context.Activities
                .OrderByDescending(e=>e.Timestamp)
                .Skip(0).Take(10).ToList();
        }
    }
}
