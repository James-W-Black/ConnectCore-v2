namespace ConnectCore_v2.Models
{
    public class Holiday
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }

        public virtual ApplicationUser ApprovedBy { get; set; }
        public virtual HolidayStatus Status { get; set; }
        public virtual User User { get; set; }


        public Holiday()
        {

        }

        public Holiday(IFormCollection form, HolidayStatus status, User user)
        {
            StartTime = DateTime.Parse(form["Holiday.StartTime"].ToString());
            EndTime = DateTime.Parse(form["Holiday.EndTime"].ToString());
            Description = form["Holiday.Description"];

            Status = status;
            User = user;
            
        }
    }
}
