using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConnectCore_v2.Models.ViewModels
{
    public class EventViewModel
    {
        public Event Event { get; set; }
        public List<SelectListItem> Location = new List<SelectListItem>();
        public List<SelectListItem> users = new List<SelectListItem>();
        public User current;
        public string LocationName  { get; set; }
        public string UserId { get; set; }
        public int numweeks { get; set; }

        public EventViewModel(Event myevent, List<Location> locations, List<User> userList, User cur , string userid)
        {
            Event = myevent;
            LocationName = myevent.Location.Name;
            UserId = userid;
            current = cur;

            foreach (var u in userList)
            {
                users.Add(new SelectListItem() { Text = u.FirstName + " " + u.LastName, Value = u.AspNetUser.Id });
            }

            foreach (var loc in locations)
            {
                Location.Add(new SelectListItem() { Text = loc.Name });
            }
        }

        public EventViewModel(List<Location> locations, List<User> userList, string userid)
        {
            UserId = userid;

            foreach (var u in userList)
            {
                users.Add(new SelectListItem() { Text = u.FirstName + " " + u.LastName, Value= u.AspNetUser.Id });
            }

            foreach (var loc in locations)
            {
                Location.Add(new SelectListItem() { Text = loc.Name });
            }
        }

        public EventViewModel()
        {

        }
    }
}
