using System.ComponentModel.DataAnnotations;

namespace ConnectCore_v2.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        //relational data
        public virtual Location Location { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Event(IFormCollection form, Location location, ApplicationUser user, DateTime start, DateTime end) 
        {
            User = user;
            Name = form["Event.Name"].ToString();
            Description = form["Event.Description"].ToString();
            StartTime = start;
            EndTime = end;
            Location = location;
        }

        public void UpdateEvent(IFormCollection form, ApplicationUser user)
        {
            User = user;
            Name = form["Event.Name"].ToString();
            Description = form["Event.Description"].ToString();
            StartTime = DateTime.Parse(form["Event.StartTime"].ToString());
            EndTime = DateTime.Parse(form["Event.EndTime"].ToString());
            
        }

        public Event()
        {

        }
    }
}
