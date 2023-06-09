using EmployeeSkilldataModels;
#nullable disable
using leave.Models.Domain;
namespace profile
{
    //For Accessing More than one model in View by Dynamic Model
    public class userprofile
    {
        public IEnumerable<Employees>skilldata{get;set;}
        public IEnumerable<Leaves>leaves{get;set;}
        
    }
}