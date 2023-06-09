global using Leave.Data;
global using System.Data.SqlClient;
global using Microsoft.AspNetCore.Mvc;
global using leave.Models.Domain;
using Microsoft.AspNetCore.Authorization;
#nullable disable
namespace admincontroller
{
[Authorize(Roles="Admin")]
public class admincontroller:Controller
{
          private LeaveDbContext _dbcontext;
          public IConfiguration Configuration{get;}
          public admincontroller(IConfiguration configuration,LeaveDbContext dbContext)
          {
              Configuration=configuration;
              _dbcontext=dbContext;
          }
          
          public IActionResult admindashboard()
          {
            string connection=Configuration["Connectionstrings:Defaultconnection"];

          using(SqlConnection connect=new SqlConnection(connection))
          {
              connect.Open();
              string employee="Select Count(*) from UserEmployee where department='developer'";
              string Hr="Select Count(*) from UserEmployee where department='HR'";
              string Manager="Select Count(*) from UserEmployee where department='Manager'";

              SqlCommand command=new SqlCommand(employee,connect);
              SqlCommand command1=new SqlCommand(Hr,connect);
              SqlCommand command2=new SqlCommand(Manager,connect);
              var result=(int)command.ExecuteScalar();
              var result1=(int)command1.ExecuteScalar();
              var result2=(int)command2.ExecuteScalar();

              TempData["empcount"]=result;
              TempData["Hrcount"]=result1;
              TempData["Managercount"]=result2;
          }
          var Toapprove=_dbcontext.leave.ToList();
          IEnumerable<Leaves>leave=from approve in Toapprove where approve.status=="not yet approved" select approve;
          TempData["Toapprovecount"]=(int)leave.Count();
              return View();
          }
}
}




        // public IActionResult adminview()
        //   {
        //     return View();
        //   } 

        // [HttpPost]
        //   public IActionResult adminview(Admin admin)
        //   {
        //     if(IsValid(admin.admin_id,admin.password))
        //     {
        //         return RedirectToAction("admindashboard","admin");
        //     }
        //     else
        //     {
        //         ModelState.AddModelError("", "Invalid user!");
        //         return View(admin);
        //     }
        //   }

        //   private bool IsValid(int admin_id,string password)
        //   {
        //       string connection=Configuration["Connectionstrings:Defaultconnection"];

        //       using(SqlConnection connect=new SqlConnection(connection))
        //       {
        //           connect.Open();
        //           SqlCommand command=new SqlCommand("Select Count(*) from admindata where admin_id=@admin_id and password=@password",connect);
        //           command.Parameters.AddWithValue("@admin_id",admin_id);
        //           command.Parameters.AddWithValue("@password",password);
        //           var result=(int)command.ExecuteScalar();
        //         return result>0;
        //       }
        //   }