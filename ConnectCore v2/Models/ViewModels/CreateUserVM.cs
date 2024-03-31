using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConnectCore_v2.Models.ViewModels
{
    public class CreateUserVM
    {     
        public User User { get; set; }
        ///*public List<SelectListItem> Location = new List<SelectListItem>()*/;
        public List<SelectListItem> UserType = new List<SelectListItem>();
        public string LocationName { get; set; }
        public string UserId { get; set; }
        public ApplicationUser aspUser  { get; set; }
        public UserType ut { get; set; }
        public bool createut;
        public CreateUserVM()
        {
            
        }

        public CreateUserVM(User myuser, List<UserType> userTypes, string userid)
        {
            User = myuser;
            LocationName = myuser.Location.Name;
            UserId = userid;

            createut = false;
            //foreach (var loc in locations)
            //{
            //    Location.Add(new SelectListItem() { Text = loc.Name });
            //}

            foreach (var ut in userTypes)
            {
                UserType.Add(new SelectListItem() { Text = ut.Name });
            }
        }


        public CreateUserVM(List<Location> locations, List<UserType> userTypes, string userid)
        {
            UserId = userid;
            createut = false;

            //foreach (var loc in locations)
            //{
            //    Location.Add(new SelectListItem() { Text = loc.Name });
            //}

            foreach (var ut in userTypes)
            {
                UserType.Add(new SelectListItem() { Text = ut.Name });
            }
        }
    


    }
}
