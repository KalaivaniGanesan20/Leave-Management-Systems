using Leavestatus;
using leavebalanceModel;
using Microsoft.AspNetCore.Authorization;
#nullable disable
namespace leaveControllers
{
  [Authorize(Roles="developer")]
    public class LeaveController:Controller
    {
        public readonly LeaveDbContext dbContext;
        public IConfiguration Configuration{get;}

        public LeaveController(LeaveDbContext dbContext)
        {
            this.dbContext=dbContext;
        }
        [HttpGet]
         public IActionResult applyleave()
         {
            Leaves leave=new Leaves();
            return View(leave);
         }

       [HttpPost]
       public async Task<IActionResult> applyleave(Leaves leaves)
       {
        if(ModelState.IsValid)
            {
              var leave=new Leaves()
              {
                  EmployeeId=leaves.EmployeeId,
                  leavetype=leaves.leavetype,
                  username=leaves.username,
                  userRole=leaves.userRole,
                  leaveReason=leaves.leaveReason,
                  leaveFrom=leaves.leaveFrom,
                  leaveTo=leaves.leaveTo,
                  Numberofdays=leaves.Numberofdays,
                  status="not yet approved"
              };
              await dbContext.leave.AddAsync(leave);
              await dbContext.SaveChangesAsync();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid input.");
                return View();
            }
            
              TempData["id"]=leaves.username;
              var leavebalance=dbContext.balance.FirstOrDefault(leave=>leave.username==TempData["id"].ToString());
              return RedirectToAction(nameof(applyleave));
       }

       public IActionResult leaveRecord()
       {
         var myleaves=dbContext.leave.ToList();
         var name=HttpContext.Session.GetString("name");
         IEnumerable<Leaves>leaves=from myleave in myleaves where myleave.username==name select myleave;
         return View(leaves);
       }
       
    
      public ActionResult leavebalance(string id)
      { 
        var leavebalance=dbContext.balance.FirstOrDefault(lb=>lb.username==id);
        var leave=dbContext.leave.ToList();
          var myleave=from leave1 in leave where leave1.username==id && leave1.status=="Approved" select leave1;

                IEnumerable<Leaves>leaves=from leave1 in myleave select leave1;
                var sick=from sickleave in leaves where sickleave.leavetype=="sick leave" select sickleave;
                var casual=from casualleave in leaves where casualleave.leavetype=="casual leave" select casualleave;
                var compensatoryoff=from compensation in leave where compensation.leavetype=="compensatoryoff" select compensation;
                var annualleave=from annual in leaves where annual.leavetype=="annualleave" select annual;
        if(leavebalance!=null)
        {
                leavebalance.sickleave=sick.Count();
                leavebalance.casualleave=casual.Count();
                leavebalance.Compensatoryleave=compensatoryoff.Count();
                leavebalance.annualleave=annualleave.Count();      
                dbContext.SaveChanges();
                return RedirectToAction("user","Login");
        }
       return RedirectToAction("user","Login");
      }

      public ActionResult leavestatus(int id)
      {
        var mystatus=dbContext.Myleavestatus.ToList();
        IEnumerable<LeavestatusTracker>status= from leave in mystatus where leave.ParameterId==id select leave;
        return View(status);
      }
     
      
    } 
}