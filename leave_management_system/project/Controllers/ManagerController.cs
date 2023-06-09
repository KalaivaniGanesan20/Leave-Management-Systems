using EmployeedataModels;
using Dapper;
using System.Data;
using Leavestatus;
using Microsoft.AspNetCore.Authorization;
using ExceptionFilters;
#nullable disable
namespace admincontroller
{
  [Authorize(Roles="Manager")]
  [CustomExceptionFilters]
 public class ManagerController:Controller
{
        public static  LeaveDbContext _dbcontext;
        public IConfiguration Configuration{get;}
        public ManagerController(IConfiguration configuration,LeaveDbContext dbContext)
        {
            Configuration=configuration;
            _dbcontext=dbContext;
        }


       public IActionResult Approving()
        {
          var leaves=_dbcontext.leave.ToList();
          IEnumerable<Leaves>myleaves =from myleave in leaves where myleave.status=="not yet approved" select myleave;
          var leave=new LeavestatusTracker();
          return View(myleaves);
        }
        public IActionResult EmployeeList()
        {
          var name=HttpContext.Session.GetString("name");
          string connection=Configuration["Connectionstrings:Defaultconnection"];
          string query = "select * from UserData where Manager=@name";

          using(SqlConnection connect=new SqlConnection(connection))
          {
            var employees = connect.Query<Employee>(query, new { name }).ToList();
            return View(employees);
          }
          }
        public IActionResult Managerdashboard()
        {
                    
          var name=HttpContext.Session.GetString("name");
 
          string connection=Configuration["Connectionstrings:Defaultconnection"];
            using(SqlConnection connect=new SqlConnection(connection))
            {
                string query="selectuser";
                var employees = connect.Query<Employee>(query, new { name }).ToList();
                IEnumerable<Employee>mylist =employees.Where(employee=>employee.Manager==name).ToList();
                List<string>Emplist=mylist.Select(employee=>employee.username).ToList();
                var sheet=_dbcontext.timesheet.ToList();
                IEnumerable<Timesheet>mysheet =sheet.ToList();
                var leaves=_dbcontext.leave.ToList();
                IEnumerable<Leaves>myleave =leaves.ToList();
                List<Leaves> filteredleave =myleave.Where(emp => Emplist.Contains(emp.username) && emp.status=="not yet approved").ToList();

                List<Timesheet> filteredEmpList =mysheet.Where(emp => Emplist.Contains(emp.username) && emp.status=="Proceed").ToList();
                 TempData["empcount"]=Emplist.Count();
                 TempData["Timesheetcount"]=filteredEmpList.Count();
                 TempData["leavecount"]=filteredleave.Count();
            }
            return View();
        } 

     [LeavestatusTracker]
      public async Task<ActionResult> Approved(int id)
      { 
        var leave= _dbcontext.leave.FirstOrDefault(leave=>leave.leaveid==id && leave.status=="not yet approved");
        var lid=leave.leaveid;
        if(leave!=null)
        {
          leave.status="Approved";
          await _dbcontext.SaveChangesAsync();
          
          return RedirectToAction("Approving","Manager");
        }
       return RedirectToAction("Approving","Manager");
      }

     [LeavestatusTracker]
      public async Task<ActionResult> Denial(int id)
      { 
        var leave= _dbcontext.leave.FirstOrDefault(leave=>leave.leaveid==id && leave.status=="not yet approved");
        var lid=leave.leaveid;
        if(leave!=null)
        {
          leave.status="Denied";
          await _dbcontext.SaveChangesAsync();
          return RedirectToAction("Approving","Manager");
        }
       return RedirectToAction("Approving","Manager");
      }
      
     public ActionResult savestatus()
     {
            // try{
              var ls=LeavestatusTracker.GetLeavstatus();
              var mystatus=_dbcontext.Myleavestatus.ToList();
              var check=mystatus.FirstOrDefault(leave=>leave.ParameterId==ls.ParameterId);
              if(check==null)
              {
                LeavestatusTracker list=new LeavestatusTracker
                {
                  name=ls.name,
                  date=ls.date,
                  ParameterId=ls.ParameterId
                };

                _dbcontext.Myleavestatus.Add(list);
                
                _dbcontext.SaveChanges();
              }
                return RedirectToAction("Approving","Manager");
            
            // catch(Exception e)
            //   {
            //     TempData["errormsg"]=e.ToString();
            //     return RedirectToAction("Approving","Manager");
            //   }
      }

    public IActionResult Timesheetapproval()
    {
          var name=HttpContext.Session.GetString("name");
 
          string connection=Configuration["Connectionstrings:Defaultconnection"];
            using(SqlConnection connect=new SqlConnection(connection))
            {
                string query="selectuser";
                var employees = connect.Query<Employee>(query, new { name }).ToList();

                IEnumerable<Employee>mylist =employees.Where(employee=>employee.Manager==name).ToList();
                List<string>Emplist=mylist.Select(employee=>employee.username).ToList();
                var sheet=_dbcontext.timesheet.ToList();
                IEnumerable<Timesheet>mysheet =sheet.ToList();

                List<Timesheet> filteredEmpList =mysheet.Where(emp => Emplist.Contains(emp.username) && emp.status=="Proceed").ToList();
                return View(filteredEmpList); 

            }
        }

        public async Task<ActionResult> Approve(int id,string status)
        { 
            var sheet= _dbcontext.timesheet.FirstOrDefault(timesheet=>timesheet.TimesheetId==id && timesheet.status=="Proceed");
            var name=sheet.username;
            ViewBag.timesheetid=sheet.TimesheetId;
           
            string connection=Configuration["Connectionstrings:Defaultconnection"];
                  using(SqlConnection connect=new SqlConnection(connection))
                  {
                      string query="selectuser";
                      var employees = connect.Query<Employee>(query, new { manager = name });
                      IEnumerable<Employee>mylist =employees.Where(emp=>emp.username==name).ToList();
                      foreach(var i in mylist)
                      {
                        ViewBag.mailid=i.emailid;
                      }
                  }

                  if(sheet!=null)
                  {  

                       if(status=="Approve")
                       {
                          sheet.status="Approve";
                          Timesheet.Acknowledgementmail(ViewBag.mailid,ViewBag.timesheetid,sheet.status);
                            var sheet1=_dbcontext.timesheet.ToList();
                            var timesheet=sheet1.FirstOrDefault(timesheet=>timesheet.TimesheetId==id);
                            var name1=timesheet.username;
                            if(timesheet.Totalhoursworked < 4)
                            {
                              var leavebalance =_dbcontext.balance.FirstOrDefault(balance=>balance.username==name1);
                              leavebalance.Lossofpay+=1;
                              _dbcontext.SaveChanges();
                            }
                                        await _dbcontext.SaveChangesAsync();
                           return RedirectToAction("Managerdashboard","Manager");
                       }
                     if(status=="Denie")
                       {
                            sheet.status="Denie";
                              Timesheet.Acknowledgementmail(ViewBag.mailid,ViewBag.timesheetid,sheet.status);
                              await _dbcontext.SaveChangesAsync();
                            return RedirectToAction("Managerdashboard","Manager");
                       }
                  }
                  return RedirectToAction("Managerdashboard","Manager");
           
        }
}
}

