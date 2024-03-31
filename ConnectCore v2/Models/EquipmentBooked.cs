namespace ConnectCore_v2.Models
{
    public class EquipmentBooked
    {
        public int Id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public virtual User User { get; set; }

    }
}
