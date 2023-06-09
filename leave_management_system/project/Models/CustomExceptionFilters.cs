using Microsoft.AspNetCore.Mvc.Filters;
#nullable disable
namespace ExceptionFilters
{
    public class CustomExceptionFilters:ExceptionFilterAttribute,IExceptionFilter
    {
        public static string error{get;set;}
       public override void OnException(ExceptionContext filterContext)   
    {  
        if (!filterContext.ExceptionHandled && filterContext.Exception is NullReferenceException)   
        {  
            error="exist";
            filterContext.ExceptionHandled = true;  
        }  
    }  
    public static string errorcode()
    {
        return error;
    }
    }
}
