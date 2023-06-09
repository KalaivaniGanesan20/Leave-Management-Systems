global using Microsoft.EntityFrameworkCore;
global using EmployeeTimesheet;
using EmployeedataModels;
using System.Data;
using SelectPdf;
using Microsoft.AspNetCore.Authorization;
using Dapper;
#nullable disable
namespace EmployeeTimesheetModels.Controllers
{
    [Authorize (Roles="Hr")]
     public class HRController:Controller
    {

       public IConfiguration Configuration{get;}
       private LeaveDbContext _context;
       public HRController(IConfiguration configuration,LeaveDbContext leavecontext)
       {
          Configuration=configuration;
          _context=leavecontext;
       }

      public IActionResult HRdashboard()
      {
          
        var name=HttpContext.Session.GetString("name");
 
        string connection=Configuration["Connectionstrings:Defaultconnection"];
        using(SqlConnection connect=new SqlConnection(connection))
        {

          string query="selectuser";
          var employees = connect.Query<Employee>(query);         
          IEnumerable<Employee>mylist =employees.Where(employee=>employee.HR==name).ToList();
          List<string>Emplist= mylist.Select(employee=>employee.username).ToList();
          TempData["namelist"]=Emplist;  

          var sheet=_context.timesheet.ToList();
          List<Timesheet> filteredEmpList =sheet.Where(employee => Emplist.Contains(employee.username) && employee.status=="not yet approved").ToList();
              
          TempData["empcount"]=Emplist.Count();
          TempData["Timesheetcount"]=filteredEmpList.Count();
          return View();
         } 
        }

       public IActionResult Report(IFormCollection form)
      {
        ViewBag.searchedvalue=form["searchedvalue"].ToString();
        
        var name=ViewBag.searchedvalue;
        var mysheets=_context.timesheet.Include(task=>task.tasks).ToList();
        IEnumerable<Timesheet> mysheet=from task in mysheets where(task.username==name)select task;
        var mytask2=_context.mytask.ToList();

        var myleavebalance=_context.balance.ToList();
        ViewBag.mybalance=myleavebalance.Where(employee=>employee.username==name).Select(leave=>new{sickleave=leave.sickleave,casualleave=leave.casualleave,annualleave=leave.annualleave,Compensatoryleave=leave.Compensatoryleave,Lossofpay=leave.Lossofpay});      
        return View(mysheet);
      }

      //html view to pdf 

       public IActionResult PrintPDF(string html)    
        {    
          html=html.Replace("StrtTag","<").Replace("EndTag",">");
          HtmlToPdf htmltopdf=new HtmlToPdf();
          PdfDocument pdfdocument=htmltopdf.ConvertHtmlString(html);
          byte[]pdf=pdfdocument.Save();
          pdfdocument.Close();
          return File(pdf,"application/pdf","privacy.pdf");
        }    

      public IActionResult EmployeeList()
      {
        var name=HttpContext.Session.GetString("name");
        string connection=Configuration["Connectionstrings:Defaultconnection"];
            using(SqlConnection connect=new SqlConnection(connection))
            {
                string query="selectuser";
                var employees = connect.Query<Employee>(query);

                IEnumerable<Employee>mylist =employees.Where(employee=>employee.HR==name).ToList();
                return View(mylist);
            }
    }

     public IActionResult TimesheetVerification()
     {
       var name=HttpContext.Session.GetString("name");
       string connection=Configuration["Connectionstrings:Defaultconnection"];
       using(SqlConnection connect=new SqlConnection(connection))
       {
        string query="selectuser";
        var employees = connect.Query<Employee>(query);
        IEnumerable<Employee>mylist =employees.Where(employee=>employee.HR==name).ToList();
        List<string>Emplist = mylist.Select(employee=>employee.username).ToList();
        TempData["namelist"]=Emplist;             
        var sheet=_context.timesheet.ToList();
        IEnumerable<Timesheet>mysheet =sheet.ToList();
        List<Timesheet> filteredEmpList =mysheet.Where(emp => Emplist.Contains(emp.username) && emp.status=="not yet approved").ToList();
        return View(filteredEmpList); 
         }
      }

    public async Task<ActionResult> proceed(int id,string status)
    { 
        var sheet= _context.timesheet.FirstOrDefault(timesheet=>timesheet.TimesheetId==id && timesheet.status=="not yet approved");
        if(sheet!=null)
        {
          if(status =="proceed")
          {
             sheet.status="Proceed";
          }
          if(status=="rework")
          {
          sheet.status="Rework";
          }

        await _context.SaveChangesAsync();
              return RedirectToAction("TimesheetVerification","HR");
        }
              return RedirectToAction("TimesheetVerification","HR");
    }
           
}
    
}