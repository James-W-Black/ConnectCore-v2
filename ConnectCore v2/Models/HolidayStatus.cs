using System.ComponentModel.DataAnnotations;

namespace ConnectCore_v2.Models
{
    public class HolidayStatus
    {
        [Key]
        public int Id { get; set;}

        public string Name { get; set;}
    }
}
