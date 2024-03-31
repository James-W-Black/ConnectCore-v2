using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConnectCore_v2.Models.ViewModels
{
    public class UserDetailsVM
    {
        public User User { get; set; }
        public List<SelectListItem> roleList = new List<SelectListItem>();
        public List<Holiday> holidays = new List<Holiday>();

        public Holiday Holiday { get; set; }
        public string RoleName { get; set; }
        public string UserId { get; set; }

        public UserDetailsVM()
        {

        }

        public UserDetailsVM(List<SelectListItem> roles, string userId)
        {
            UserId = userId;

            foreach(var r in roles)
            {
                roleList.Add(r);
            }
        }

        public UserDetailsVM(User myuser,List<SelectListItem> roles, string userId, List<Holiday> hols)
        {
            User = myuser;
            UserId = userId;
            roleList = roles;

            foreach(var h in hols)
            {
                holidays.Add(h);
            }

            //foreach (var r in roles)
            //{
            //    roleList.Add(r);
            //}
        }
    }
}
