namespace ConnectCore_v2.Models
{
    public class UserToUserType
    {
        public int Id { get; set;}

        public virtual ApplicationUser User { get; set; }
        public virtual UserType UserType { get; set; }
    }
}
