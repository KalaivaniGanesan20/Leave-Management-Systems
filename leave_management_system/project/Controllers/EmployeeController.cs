using leavebalanceModel;
using EmployeedataModels;
using System.Data;
using Dapper;
#nullable disable
namespace Employeedata.Controllers
{
    public class EmployeeController:Controller
    { 
        public readonly LeaveDbContext dbContext;
        public IConfiguration Configuration{get;}
        private readonly IWebHostEnvironment webHostEnvironment;
        public List<Employee>employeelist=new List<Employee>();

        public EmployeeController(IConfiguration configuration,IWebHostEnvironment webhost,LeaveDbContext dbContext)
        {
            Configuration=configuration;
            this.dbContext=dbContext;
            webHostEnvironment=webhost;
        }

        public IActionResult CheckId(int Id)
        {
             string connection=Configuration["Connectionstrings:Defaultconnection"]; 
                 using(SqlConnection connect=new SqlConnection(connection))
                 {
                        string sql="select count(*) from UserEmployee where Id=@id";
                        connect.Open();
                        SqlCommand checkcommand=new SqlCommand(sql,connect);
                        checkcommand.Parameters.AddWithValue("@id",Id);
                        int employeeexit=(int)checkcommand.ExecuteScalar();

                        connect.Close();

                        //  check user already exit or not

                        if(employeeexit==0)
                        {
                           
                            return Json(true);
                        }
                        else
                        {
                             return Json($"Id {Id} is already taken");
                        }
        }
        }
         public IActionResult Checkusername(string username)
        {
             string connection=Configuration["Connectionstrings:Defaultconnection"]; 
                 using(SqlConnection connect=new SqlConnection(connection))
                 {
                        string sql="select count(*) from UserEmployee where username=@username";
                        connect.Open();
                        SqlCommand checkcommand=new SqlCommand(sql,connect);
                        checkcommand.Parameters.AddWithValue("@username",username);
                        int employeeexit=(int)checkcommand.ExecuteScalar();

                        connect.Close();

                        //  check user already exit or not

                        if(employeeexit==0)
                        {
                           
                            return Json(true);
                        }
                        else
                        {
                             return Json($"Username {username} is already taken");
                        }
        }
        }
        
        // Generating path for uploaded images to store in Databse
        public  string Uploadedfiles( IFormFile ImageUrl)
        {
            string UniqueFile=null;
            if(ImageUrl!=null)
            {
            string uploadfolder=Path.Combine(webHostEnvironment.WebRootPath,"Images");
            string extension = Path.GetExtension(ImageUrl.FileName);
            UniqueFile = DateTime.Now.Ticks.ToString() + new Random().Next().ToString() + extension;
            string path=Path.Combine(uploadfolder,UniqueFile);
            using(var Filestream=new FileStream(path,FileMode.Create))
            {
                ImageUrl.CopyTo(Filestream);
            }
            }
            return UniqueFile;
        }

       // Add Employee Action
        public IActionResult creates()
        {
            return View();
        }

      [HttpPost]
        public IActionResult creates(Employee employee)
        {
            TempData["empid"]=employee.Id;

            if(ModelState.IsValid)
            {
                 string connection=Configuration["Connectionstrings:Defaultconnection"];
 
                 using(SqlConnection connect=new SqlConnection(connection))
                 {
                        string sql="select count(*) from UserEmployee where id=@id";
                        connect.Open();
                        SqlCommand checkcommand=new SqlCommand(sql,connect);
                        checkcommand.Parameters.AddWithValue("@id",employee.Id);
                        checkcommand.Parameters.AddWithValue("@username",employee.username);
                        int employeeexit=(int)checkcommand.ExecuteScalar();
                        connect.Close();

                        //  check user already exit or not

                        // if(employeeexit > 0)
                        // {
                        //     ModelState.AddModelError(string.Empty, "user already exists.");
                        //     ViewBag.exist="user already exists.";
                        //     return View();
                        // }
                        // else
                        // {
                            string insert="Employeedata";
                            SqlCommand command=new SqlCommand(insert,connect);
                            command.CommandType=CommandType.StoredProcedure;
                            
                            command.Parameters.AddWithValue("@id", employee.Id);
                            command.Parameters.AddWithValue("@username", employee.username);
                            command.Parameters.AddWithValue("@Firstname", employee.Firstname);
                            command.Parameters.AddWithValue("@Lastname", employee.Lastname);
                            command.Parameters.AddWithValue("@userpassword",employee.userpassword);
                            command.Parameters.AddWithValue("@dateofbirth", employee.dateofbirth);
                            command.Parameters.AddWithValue("@contact_number", employee.contact_number);
                            command.Parameters.AddWithValue("@emailid", employee.emailid);
                            command.Parameters.AddWithValue("@HR", employee.HR);
                            command.Parameters.AddWithValue("@Manager", employee.Manager);
                            command.Parameters.AddWithValue("@dateofjoined", employee.dateofjoined);
                            command.Parameters.AddWithValue("@department",employee.department);
                            string UniqueFileName=Uploadedfiles(employee.ImageUrl);
                            employee.ImageFile=UniqueFileName;
                            command.Parameters.AddWithValue("@ImageFile",employee.ImageFile);
                            command.Parameters.AddWithValue("@salary",employee.salary);
                            connect.Open();
                            command.ExecuteNonQuery();
                            connect.Close();

                            // Once user is Registered Balance sheet for particular user is created

                             var leave=new leavebalance
                            {
                                username=employee.username,
                                Employeeid=employee.Id
                            };                            
                            dbContext.balance.Add(leave);
                            dbContext.SaveChanges(); 
                            return RedirectToAction("admindashboard","admin");
                            
                        }
                 
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid input.");
                return View();
            }
        }

         // List of Registered Employee data

        public IActionResult Index()
        {
            string connection=Configuration["Connectionstrings:Defaultconnection"];

            using(SqlConnection connect=new SqlConnection(connection))
            {
                string query="selectuser";
                var employees = connect.Query<Employee>(query);
                return View(employees);
            }
           
        }
        
        // Search Employee using Name

        public List<Employee>searchedemployee=new List<Employee>();
          public IActionResult search(IFormCollection form)
        {
            ViewBag.searchvalue=form["searchvalue"].ToString();
            string connection=Configuration["Connectionstrings:Defaultconnection"];
            
            using(SqlConnection connect=new SqlConnection(connection))
            {
                string query="selectuser";
                var employees = connect.Query<Employee>(query);
                foreach(var employee in employees)
                {
                    if(employee.username.Contains(ViewBag.searchvalue))
                        {
                            searchedemployee.Add(employee);
                        }
                }
            }

            return View(searchedemployee);
        }

       //update Employee Details
        public IActionResult Update(int id)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            Employee employee = new Employee();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * From UserEmployee Where Id='{id}'";
                SqlCommand command = new SqlCommand(sql, connection);

                connection.Open();

                using (SqlDataReader datarow = command.ExecuteReader())
                {
                    while (datarow.Read())
                    {
                        employee.Id=Convert.ToInt32(datarow["Id"]);
                        employee.username=Convert.ToString(datarow["username"]);
                        employee.Firstname=Convert.ToString(datarow["Firstname"]);
                        employee.Lastname=Convert.ToString(datarow["Lastname"]);
                        employee.Lastname=Convert.ToString(datarow["HR"]);
                        employee.Lastname=Convert.ToString(datarow["Manager"]);
                        employee.userpassword=Convert.ToString(datarow["userpassword"]);
                        employee.emailid=Convert.ToString(datarow["emailid"]);
                        employee.contact_number=Convert.ToString(datarow["contact_number"]);
                        employee.department=Convert.ToString(datarow["department"]);
                        employee.salary=Convert.ToInt32(datarow["salary"]);
                        employee.ImageFile=Convert.ToString(datarow["ImageFile"]);
                    }
                }

                connection.Close();
            }
            return View(employee);
        }

        [HttpPost]
        public IActionResult Update(Employee employee , int id)
        {
            string connection=Configuration["Connectionstrings:Defaultconnection"];
            using(SqlConnection sqlcon=new SqlConnection(connection))
            {
                string sql="Update UserEmployee Set Id=@Id ,username=@username,Firstname=@Firstname,Lastname=@Lastname,userpassword=@userpassword,emailid=@emailid,HR=@HR,Manager=@Manager,contact_number=@contact_number,department=@department,salary=@salary,ImageFile=@ImageFile where Id=@id";
                        SqlCommand command = new SqlCommand(sql,sqlcon);
                        command.Parameters.AddWithValue("@id", employee.Id);
                        command.Parameters.AddWithValue("@username", employee.username);
                        command.Parameters.AddWithValue("@Firstname", employee.Firstname);
                        command.Parameters.AddWithValue("@Lastname", employee.Lastname);
                        command.Parameters.AddWithValue("@HR", employee.HR);
                        command.Parameters.AddWithValue("@Manager", employee.Manager);
                        command.Parameters.AddWithValue("@userpassword", employee.userpassword);
                        command.Parameters.AddWithValue("@contact_number", employee.contact_number);
                        command.Parameters.AddWithValue("@emailid", employee.emailid);
                        command.Parameters.AddWithValue("@department",employee.department);
                        string UniqueFileName=Uploadedfiles(employee.ImageUrl);
                        employee.ImageFile=UniqueFileName;
                        command.Parameters.AddWithValue("@ImageFile",employee.ImageFile);
                        command.Parameters.AddWithValue("@salary",employee.salary);
                                     
                        sqlcon.Open();
                        command.ExecuteNonQuery();
                        sqlcon.Close();
                    
            }
            return RedirectToAction("Index","Employee");
        }
       
    //Delete Registered User

        public ActionResult Delete(int id)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

                Employee employee = new Employee();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql ="delete  From UserEmployee Where Id=@id";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@id",id);               

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return RedirectToAction("Index","Employee");
        }   
    
   }
}