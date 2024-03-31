using System.ComponentModel.DataAnnotations;

namespace ConnectCore_v2.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ApplicationUser AspNetUser { get; set; }

        public virtual UserType UserType { get; set; }
        public virtual Location Location { get; set; }

        public User(IFormCollection form, Location location, UserType usertype, ApplicationUser user)
        {
            AspNetUser = user;
            FirstName = form["User.FirstName"].ToString();
            LastName = form["User.LastName"].ToString();
            Location = location;
            UserType = usertype;
        }

        public User(string fName, string lName, Location location, UserType usertype, ApplicationUser user)
        {
            AspNetUser = user;
            FirstName = fName;
            LastName = lName;
            Location = location;
            UserType = usertype;
        }

        public User()
        {

        }
    }
}
