namespace ApiApplicationProject.Models
{
    public class ActionModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser UName { get; set; }

    }
}
