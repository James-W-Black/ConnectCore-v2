using ConnectCore_v2.Models;

namespace ConnectCore_v2.Helpers
{
    public static class JSONListHelper
    {
        public static string GetEventListJSONString(List<ConnectCore_v2.Models.Event> events)
        {
            var eventList = new List<Event>();
            
            foreach(var model in events)
            {
                var myevent = new Event()
                {
                    id = model.Id,
                    start = model.StartTime,
                    end = model.EndTime,
                    resourceId = model.Location.Id,
                    description = model.Description,
                    title = model.Name,
                    color = "#2d4052"
                };
                eventList.Add(myevent);
            }

            return System.Text.Json.JsonSerializer.Serialize(eventList);
        }

        public static string GetEquipmentListJSONString(List<ConnectCore_v2.Models.EquipmentBooked> equipment)
        {
            var equipList = new List<EquipmentBooked>();

            foreach(var model in equipment)
            {
                var myequip = new EquipmentBooked()
                {
                    id = model.Id,
                    start = model.start,
                    end = model.end,
                    user = model.User.Id
                };

                equipList.Add(myequip);
            }
            return System.Text.Json.JsonSerializer.Serialize(equipList);
        }

        public static string GetHolListJSONString(List<ConnectCore_v2.Models.Holiday> hol)
        {
            var holList = new List<holiday>();

            foreach (var model in hol)
            {
                var myHol = new holiday()
                {
                    id = model.Id,
                    start = model.StartTime,
                    end = model.EndTime,
                    description = model.Description,
                    approvedBy = model.ApprovedBy.Id,
                    color = "#4dffff"
                };
                holList.Add(myHol);
            }

            return System.Text.Json.JsonSerializer.Serialize(holList);
        }

        public static string GetResourceListJSONString(List<Models.Location> locations)
        {
            var resourceList = new List<Resource>();
            foreach(var location in locations)
            {
                var resource = new Resource()
                {
                    id = location.Id,
                    title = location.Name
                };
                resourceList.Add(resource);
            }
            return System.Text.Json.JsonSerializer.Serialize(resourceList);
        }
    }

    public class Event
    {
        public int id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int resourceId { get; set; }
        public string description { get; set; }    
        public string title { get; set; }
        public string color { get; set; }
    }

    public class EquipmentBooked
    {
        public int id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int user { get; set; }
    }

    public class holiday
    {
        public int id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string description { get; set; }
        public string approvedBy { get; set; }
        public string color { get; set; }
    }

    public class Resource
    {
        public int id { get; set; }
        public string title { get; set; }
            
    }
}
