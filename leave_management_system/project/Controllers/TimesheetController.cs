using System.Globalization;
using Microsoft.AspNetCore.Authorization;
#nullable disable
namespace EmployeeTimesheetModels.Controllers
{
  [Authorize(Roles="developer")]
    public class TimesheetController:Controller
    {
       
       private LeaveDbContext _context;
       public TimesheetController(LeaveDbContext leavecontext)
       {
          _context=leavecontext;
       }

        public IActionResult Index()
       {        
          TempData["thw"]="-";
          DateTime startOfMonth = new DateTime(DateTime.Today.Year,DateTime.Today.Month,1);
          DateTime enddate = startOfMonth.AddMonths(1).AddDays(-1);
          List<DateTime> Dates = new List<DateTime>();
          for (DateTime date = startOfMonth; date <= enddate; date = date.AddDays(1))
          {
              Dates.Add(date);
          }
          Dates.RemoveAll(date=>date.DayOfWeek==DayOfWeek.Sunday);
          return View(Dates);
       }

       [HttpGet]
       [Route("Timesheet/timesheet/{id}")]
       public IActionResult timesheet(string id)
       {      
         Timesheet timesheet=new Timesheet();

         DateTime mydate = DateTime.ParseExact(id.Replace("%2F", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

         ViewBag.applydate=mydate.ToString("MM/dd/yyyy");
         Random rand = new Random();
         int randomNum = rand.Next();
         ViewBag.SheetId=randomNum;

        return View(timesheet);
       }
     
      [HttpPost]
      public ActionResult timesheet(Timesheet timesheet)
      {
        
        var mysheet1=new Timesheet()
        {
            TimesheetId=timesheet.TimesheetId,
            username=timesheet.username,
            projectname=timesheet.projectname,
            date=timesheet.date,
            status="not yet approved"
        };     
        ViewBag.TimesheetId=timesheet.TimesheetId; 
        ViewBag.username=HttpContext.Session.GetString("name");
        _context.timesheet.Add(mysheet1);
        _context.SaveChanges();
        return View(nameof(timesheet));
      }

     [HttpGet]
     [Route("Timesheet/addtask/{id:int:maxlength(15)}")]
     public IActionResult addtask(int id)
     {       
      MyTask task=new MyTask();
      ViewBag.TimesheetId=id;
      return View(task);
     }

  
     [HttpPost]
      public async Task<IActionResult> addtask(MyTask mytasks)
      {        
        var Timesheet=_context.timesheet.FirstOrDefault(timesheet=>timesheet.TimesheetId==mytasks.TimesheetId);
       
          var name=HttpContext.Session.GetString("name");
          var mytasks1=new MyTask()
          {
              taskid=Guid.NewGuid(),
              taskname=mytasks.taskname,
              username=name,
              start_time=mytasks.start_time,
              end_time=mytasks.end_time,
              hoursworked=mytasks.hoursworked,
              taskstatus=mytasks.taskstatus,
              TimesheetId=mytasks.TimesheetId,
          };
        Timesheet.Totalhoursworked+=mytasks.hoursworked;
        await  _context.mytask.AddAsync(mytasks1);
        await _context.SaveChangesAsync();
        return View(nameof(addtask));
        }
    public IActionResult Mytask()
    {
        var name=HttpContext.Session.GetString("name");
        var mysheets=_context.timesheet.Include(t=>t.tasks).ToList();
        IEnumerable<Timesheet> mysheet=from t in mysheets where(t.username==name)select t;
        var mytask2=_context.mytask.ToList();

        var myleavebalance=_context.balance.ToList();
        ViewBag.mybalance=myleavebalance.Where(e=>e.username==name).Select(l=>new{sickleave=l.sickleave,casualleave=l.casualleave,annualleave=l.annualleave,Compensatoryleave=l.Compensatoryleave,Lossofpay=l.Lossofpay});      
        return View(mysheet);
    }

    }
}

