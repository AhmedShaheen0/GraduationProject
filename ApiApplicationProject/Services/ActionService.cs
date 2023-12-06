using ApiApplicationProject.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiApplicationProject.Services
{
    public class ActionService : IActionService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ActionService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public async Task<ActionModel> CreateEvent(EvntModel ev, string userId)
        {
            

            var model = new ActionModel
            {
               ApplicationUserId= userId,
               StartTime= DateTime.UtcNow,
               EndTime= DateTime.UtcNow,
               Title= ev.Title,
               
            };
            _context.Actions.Add(model);
            _context.SaveChanges();
            return model;
        }

        public ActionModel GetEventById(int id)
        {
         
            
            return _context.Actions.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<ActionModel> GetEvents()
        {
            return _context.Actions.ToList();
        }
    }
}
