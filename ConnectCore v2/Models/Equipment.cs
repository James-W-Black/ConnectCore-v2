namespace ConnectCore_v2.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Location Location { get; set; }
        
    }
}
