using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Leavestatus
{
    // Leave status tracker to track controller and action name(Approved/Denied action) and date.

    public class LeavestatusTracker:ActionFilterAttribute
    {      
        [Key]
        public int statusid{get;set;}

        public string name{get;set;}

        public DateTime date{get;set;}

        public int ParameterId{get;set;}

        public static List<LeavestatusTracker>LeaveStatus=new List<LeavestatusTracker>();
        public static LeavestatusTracker mystatus;


       public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var parameters = filterContext.HttpContext.Request.RouteValues;
            var id = parameters["id"];
            mystatus=new LeavestatusTracker
            {
            name=filterContext.ActionDescriptor.DisplayName,
            date=DateTime.Now,
            ParameterId = id != null ? int.Parse(id.ToString()) : 0 // Parse id to int, or use 0 if it's null
            };
            LeaveStatus.Add(mystatus);
            base.OnActionExecuted(filterContext);
        }
        
        public static List<LeavestatusTracker> Getlist()
        {
            return LeaveStatus;
        }
        public static LeavestatusTracker GetLeavstatus()
        {
            return mystatus;
        }
    }
}