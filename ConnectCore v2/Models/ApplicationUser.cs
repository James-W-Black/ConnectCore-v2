using Microsoft.AspNetCore.Identity;


namespace ConnectCore_v2.Models
{
    public class ApplicationUser : IdentityUser
    {
        //public string FirstName { get; set; }
        //public string LastName { get; set; }

        
      
        public virtual ICollection<Event> Events { get; set; }
        
    }
}
