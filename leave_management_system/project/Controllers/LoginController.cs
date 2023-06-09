using System.Data;
using EmployeedataModels;
using Newtonsoft.Json;
using EmployeeSkilldataModels;
using profile;
using Microsoft.AspNetCore.Identity;
using Dapper;
#nullable disable
namespace UserLogin.Controllers
{
  public  class LoginController:Controller
    {
      //congiguration of database
      public IConfiguration Configuration{get;}
       private readonly SignInManager<IdentityUser> _signInManager;

       public LeaveDbContext _dbContext{get;set;}
        public LoginController(IConfiguration configuration,LeaveDbContext dbcontext,SignInManager<IdentityUser> signInManager)
        {
            Configuration=configuration;
            _dbContext=dbcontext;
             _signInManager = signInManager;
        }
       public async Task<IActionResult>user()
       {
            var name=HttpContext.Session.GetString("name");
            var leave=_dbContext.leave.Where(l=>l.username==name).ToList();
            var date=DateTime.Now.ToString("MM/dd/yyyy");
            var timesheet=_dbContext.timesheet.Where(sheet=>sheet.date ==date && sheet.username==name);

            if(timesheet.Count()==0)
            {
              ViewBag.remembersheet="true";
            }
            else
            {
              ViewBag.remembersheet="false";
            }

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            //Consuming HttpGet FRom web api

            using(var client = new HttpClient(clientHandler))
            {                
                var response = await client.GetAsync("https://localhost:7027/api/Employees");
                var json = await response.Content.ReadAsStringAsync();
                var uname=HttpContext.Session.GetString("name");
                var data = JsonConvert.DeserializeObject<List<Employees>>(json);
                data=data.Where(e=>e.name.ToLower()==uname.ToLower()).ToList();            
               
               //Object for dynamic class

                var profiledata=new userprofile()
                {
                    skilldata=data,
                    leaves=leave
                };
                
                  
                  return View(profiledata);
            }
            
         }
         
       public IActionResult profile()
        {
                    
          // if session ended 

            if(HttpContext.Session.GetString("name")==null)
            {
              throw new Exception("Session End!!!");
            }

          // To Get all employee data and stored in List
             var name=HttpContext.Session.GetString("name");
             string connection=Configuration["Connectionstrings:Defaultconnection"];
              using(SqlConnection connect=new SqlConnection(connection))
              {
                  string query=$"Select * From UserEmployee Where username='{name}'";
                  var employees = connect.Query<Employee>(query);

                   return View(employees);
              }
          }

          public async Task<ActionResult> Logout()
          {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
          }
    }

}


















  //list that stores all employee data from database


      // cookies with rememberme
     
      //  public IActionResult loginview()
      //  {
      //    if(Request.Cookies["username"]!=null && Request.Cookies["password"]!=null)
      //    {
      //      TempData["uname"]=Request.Cookies["username"];
      //       TempData["password"]=Request.Cookies["password"];
      //    }
      //    return View();
      //  }


  //  public IActionResult Remove()
      //   {
      //     //end session if user logout

      //       ISession session=HttpContext.Session;
      //       session.Clear();
      //       return RedirectToAction("Index","Home");
      //   }
      //  [HttpPost]
      //  public IActionResult loginview(IFormCollection login)
      //  {


      //    if(IsValid(login["username"],login["password"],login["department"]))
      //    {
      //         //Cookies
      //         CookieOptions options=new CookieOptions();
      //        if(login["rememberMe"]!=false)
      //        {
      //         options.Expires=DateTime.Now.AddMinutes(1);
      //         Response.Cookies.Append("username",login["username"],options);
      //         Response.Cookies.Append("password",login["password"],options);
      //        }

      //        // set session
      //         HttpContext.Session.SetString("name",login["username"]);
             
      //        // invoke logger

      //         var logger=new MyLogger();
      //         logger.log(login["username"]);
      //         return RedirectToAction("Index","Home");
      //    }
      // else
      //    {
      //       //generate eroor message

      //       ModelState.AddModelError("", "Invalid user!");
      //       return View("loginview");
      //    }
         
      //  }
      // private bool IsValid(string username,string password,string department)
      //  {
      //    string connection=Configuration["Connectionstrings:Defaultconnection"];
      //    HttpContext.Session.SetString("Role",department);
      //    using(SqlConnection connect=new SqlConnection(connection))
      //    {
      //       connect.Open();
      //       SqlCommand command=new SqlCommand("Select Count(*) from UserLoginData where department=@department and name=@username and userpassword=@password",connect);
      //       command.Parameters.AddWithValue("@username",username);
      //       command.Parameters.AddWithValue("@department",department);
      //       command.Parameters.AddWithValue("@password",password);
      //       var result=(int)command.ExecuteScalar();
      //      return result>0;
      //    }
         
      //  }
      