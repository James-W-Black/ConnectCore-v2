using System.ComponentModel.DataAnnotations;

namespace ConnectCore_v2.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        //relational data    
        
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public Location(string locname)
        {
            Name = locname;
        }

        public Location()
        {

        }
    }
}
