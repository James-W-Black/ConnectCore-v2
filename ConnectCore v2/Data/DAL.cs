using ConnectCore_v2.Models;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Security.Claims;

namespace ConnectCore_v2.Data
{
    public interface IDAL
    {
        //Events
        public Event GetEvent(int id);
        public List<Event> GetEvents(Location loc);   
        public List<Event> GetMyEvents(string userId);
        public List<Equipment> GetEquipment(Location loc);
        public void CreateEvent(IFormCollection form, Location loc, ApplicationUser assignTo);
        public void CreateUser(IFormCollection form);
        public void CreateEquipment(IFormCollection form, Location loc);
        public void CreateUserType(string name);
        public void CreateRepeatingshift(IFormCollection form, Location loc, ApplicationUser assignTo);
        public void CreateOtherUser(IFormCollection form, Location loc);
        public void updateEvent(IFormCollection form);
        public void DeleteEvent(int id);
        public User CreateUser(string fName);
        public List<IdentityRole> GetRoles();
        public List<UserType> GetUserTypes();
        public List<Holiday> GetHolidays(int userId);
        public List<Holiday> GetHolidaysForApproval();
        public List<EquipmentBooked> GetEquipmentBooked(Location loc);
        public List<Event> GetUsersFutureEvents(string Id);
        public List<Holiday> GetApprovedHolidays();
        public void removeEvent(Event e);
        public List<Holiday> GetHolsById(List<String> Ids);
        public void CancelHolidays(List<Holiday> holList);
        public List<Holiday> GetApprovedHolForUser(string Id);
        public List<Holiday> GetHolsForUser(User Id);
        public void CancelHoliday(Holiday holList);
        public void ApproveHoliday(Holiday hol, ApplicationUser user);
        public User GetUserByAspNetId(string id);
        public bool isUserInRole(string roleName, int userId);
        public void CreateHoliday(IFormCollection form);
        public User GetUserByUserId(int id);
        public List<User> GetUsers(Location loc);
        public Holiday GetholById(int id);
        public ApplicationUser GetUserById(String userId);
        
        //Location
        public Location GetLocation(int id);
        public Location GetLocation(string locName);
        public Location GetLoggedInUserLocation(int id);
        public List<Location> GetLocations();
        public void CreateLocation(Location location);

        //public async Task<Location> CreateLocation2(string location);
    }

    public class DAL : IDAL
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Events
        public Event GetEvent(int id)
        {
            return db.Events.FirstOrDefault(x => x.Id == id);
        }

        public List<IdentityRole> GetRoles()
        {
            return db.Roles.ToList();
        }

        public List<Holiday> GetHolidaysForApproval()
        {
            var status = db.HolidayStatus.FirstOrDefault(x => x.Name == "Awaiting Approval");
            List<Holiday> hols = db.Holiday.Where(x => x.Status.Id == status.Id).ToList();
            return hols;
        }

        public List<Holiday> GetHolsById(List<String> Ids)
        {
            List<Holiday> hols = new List<Holiday>();

            foreach(var id in Ids)
            {
                hols.Add(db.Holiday.FirstOrDefault(x => x.Id == Convert.ToInt32(id)));
            }

            return hols;
        }

        public List<Holiday> GetApprovedHolidays()
        {
            var status = db.HolidayStatus.FirstOrDefault(x => x.Name == "Approved");
            List<Holiday> hols = db.Holiday.Where(x => x.Status.Id == status.Id).ToList();
            return hols;
        }

        public Location GetLoggedInUserLocation(int id)
        {
            return db.User.FirstOrDefault(x => x.Id == id).Location;
        }
        public bool isUserInRole(string role, int userId)
        {
            var user = db.User.FirstOrDefault(x => x.Id == userId);
            //var aspuser = db.Users.FirstOrDefault(x => x.Id == user.AspNetUser.Id);

            var roleId = db.Roles.FirstOrDefault(x => x.Name == role).Id;
            var findRole = db.UserRoles.FirstOrDefault(p => p.UserId == user.AspNetUser.Id && p.RoleId == roleId);
            return db.UserRoles.Contains(findRole);
        }

        public List<Event> GetEvents(Location Loc)
        {
            return db.Events.Where(x => x.Location == Loc).ToList();
        }

        public List<UserType> GetUserTypes()
        {
            return db.UserTypes.ToList();
        }

        public void removeEvent(Event e)
        {           
            db.Events.Remove(e);
            db.SaveChanges();
        }

        public List<Event> GetMyEvents(string userId)
        {
            return db.Events.Where(x => x.User.Id == userId).ToList();
        }
        public User GetUserByAspNetId(string id)
        {
            return db.User.FirstOrDefault(x => x.AspNetUser.Id == id);
        }

        public User GetUserByUserId(int id)
        {
            return db.User.FirstOrDefault(x => x.Id == id);
        }

        public List<User> GetUsers(Location loc)
        {
            return db.User.Where(x=>x.Location == loc).ToList();
        }

        public ApplicationUser GetUserById(String userId)
        {
            return db.Users.FirstOrDefault(x => x.Id == userId);
        }



        public void CreateEvent(IFormCollection form, Location loc, ApplicationUser assignTo)
        {
            //var locname = form["Location"].ToString();
            //var user = db.Users.FirstOrDefault(x => x.Id == form["UserId"].ToString());
            DateTime start = DateTime.Parse(form["Event.StartTime"].ToString());
            DateTime end = DateTime.Parse(form["Event.EndTime"].ToString());

            var newevent = new Event(form, loc, assignTo, start,end);
            db.Events.Add(newevent);
            db.SaveChanges();
        }

        public void CreateRepeatingshift(IFormCollection form, Location loc, ApplicationUser assignTo)
        {
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            int numofweeks = Convert.ToInt32(form["numweeks"]);

            DateTime start = DateTime.Parse(form["Event.StartTime"].ToString());
            DateTime end = DateTime.Parse(form["Event.EndTime"].ToString());

            for (int i = 0; i < numofweeks; i++)
            {
                var newevent = new Event(form, loc, assignTo, start, end);

                db.Events.Add(newevent);
                db.SaveChanges();

                start = myCal.AddWeeks(start, 1);
                end = myCal.AddWeeks(end, 1);
            }
        }

        public List<Holiday> GetHolidays(int userId)
        {
            return db.Holiday.Where(x => x.User.Id == userId).ToList();
        }

        public Holiday GetholById(int id)
        {
            return db.Holiday.FirstOrDefault(x => x.Id == id);
        }

        public void CreateHoliday(IFormCollection form)
        {
            //status = status where name is awaiting approval
            //get the current user
           var status = db.HolidayStatus.FirstOrDefault(x => x.Name == "Awaiting Approval");
            var user = db.User.FirstOrDefault(x => x.AspNetUser.Id == form["UserId"].ToString());
           var newHoliday = new Holiday(form, status,user);
            db.Holiday.Add(newHoliday);
            db.SaveChanges();
        }

        public void ApproveHoliday(Holiday hol, ApplicationUser user)
        {
            var status = db.HolidayStatus.FirstOrDefault(x => x.Name == "Approved");

                    
                hol.Status = status;
                hol.ApprovedBy = user;
            

            db.SaveChanges();
        }

        public List<Holiday> GetApprovedHolForUser(string Id)
        {
            var user = db.User.FirstOrDefault(x=>x.AspNetUser.Id == Id);
            var status = db.HolidayStatus.FirstOrDefault(x => x.Name == "Approved");
            var list = db.Holiday.Where(x => x.User.Id == user.Id && x.Status == status).ToList();
            return db.Holiday.Where(x => x.User.Id == user.Id && x.Status == status).ToList();
        }

        public List<Holiday> GetHolsForUser(User user)
        {
            var status = db.HolidayStatus.FirstOrDefault(x => x.Name == "Cancelled");
            return db.Holiday.Where(x=>x.User.Id == user.Id && x.Status != status).ToList();
        }

        public List<Event> GetUsersFutureEvents(string Id)
        {
            return db.Events.Where(x => x.StartTime >= DateTime.Now).ToList();
           
        }

        public void CancelHoliday(Holiday hol)
        {
            var status = db.HolidayStatus.FirstOrDefault(x => x.Name == "Cancelled");

            hol.Status = status;

            db.SaveChanges();
        }

        public void CreateUserType(string name)
        {
            UserType usertype = new UserType();
            usertype.Name = name;
            db.UserTypes.Add(usertype);
            db.SaveChanges();
        }

        public void CancelHolidays(List<Holiday> holList)
        {
            var status = db.HolidayStatus.FirstOrDefault(x => x.Name == "Cancelled");

            foreach (var hol in holList)
            {
                hol.Status = status;
            }

            db.SaveChanges();
        }

        public void CreateEquipment(IFormCollection form, Location loc)
        {
            Equipment equipment = new Equipment();
            equipment.Name = form["Name"];
            equipment.Location = loc;

            db.Equipment.Add(equipment);
            db.SaveChanges();
        }

        public List<Equipment> GetEquipment(Location loc)
        {
            return db.Equipment.Where(x => x.Location == loc).ToList();
        }

        public List<EquipmentBooked> GetEquipmentBooked(Location loc)
        {
            return db.EquipmentBooked.Where(x => x.User.Location == loc).ToList();
        }

        public User CreateUser(string fName)
        {
            var newuser = new User();
            db.User.Add(newuser);
            db.SaveChanges();

            return newuser;
        }

        public void CreateUser(IFormCollection form)
        {
            var locname = form["LocationName"].ToString();

            Location loc = db.Locations.FirstOrDefault(x => x.Name == locname);

            //if loc does not exist create it
            if (loc == null)
            {               
                CreateLocation(new Location(locname));
                loc = db.Locations.FirstOrDefault(x => x.Name == locname);
            }

            string typename;

            if(form["ut.Name"] != string.Empty)
            {
                typename = form["ut.Name"].ToString();
                CreateUserType(typename);
            }
            else
            {
                 typename = form["UserType"].ToString();
            }

            
            var user = db.Users.FirstOrDefault(x => x.Id == form["UserId"].ToString());
            var newuser = new User(form, loc, db.UserTypes.FirstOrDefault(x => x.Name == typename), user);
            db.User.Add(newuser);
            db.SaveChanges();
        }

        public void CreateOtherUser(IFormCollection form, Location loc)
        {
            string typename;

            if (form["ut.Name"] != string.Empty)
            {
                typename = form["ut.Name"].ToString();
                CreateUserType(typename);
            }
            else
            {
                typename = form["UserType"].ToString();
            }

      
            var user = db.Users.FirstOrDefault(x => x.Id == form["UserId"].ToString());
            var newuser = new User(form, loc, db.UserTypes.FirstOrDefault(x => x.Name == typename), user);
            db.User.Add(newuser);
            db.SaveChanges();
        }




        public void updateEvent(IFormCollection form)
        {           
            var eventid = int.Parse(form["Event.Id"]);
            var myEvent = db.Events.FirstOrDefault(x => x.Id == eventid);
            
            var user = db.Users.FirstOrDefault(x => x.Id == form["User"].ToString());
            myEvent.UpdateEvent(form, user);
            db.Entry(myEvent).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteEvent(int id)
        {
            var myEvent = db.Events.Find(id);
            db.Events.Remove(myEvent);
            db.SaveChanges();
        }

        //Location
        public Location GetLocation(int id)
        {
            return db.Locations.Find(id);
        }

        public Location GetLocation(string locName)
        {
            return db.Locations.Find(locName);
        }

        public List<Location> GetLocations()
        {
            return db.Locations.ToList();
        }

        public void CreateLocation(Location location)
        {
            db.Locations.Add(location);
            db.SaveChanges();
        }

        //public  Task<Location> CreateLocation2(string location)
        //{
        //    Location loc = new Location();
        //    loc.Name = location;
        //    db.Locations.Add(loc);
        //    db.SaveChanges();
        //    return loc;
        //}

    }
}
