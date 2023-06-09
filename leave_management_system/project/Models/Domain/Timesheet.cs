using System.Net.Mail;
using System.Net;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace EmployeeTimesheet
{
    public class Timesheet
    {
                
                
        [Required]
        public int TimesheetId{get;set;}

        [Required]
        public string username{get;set;}

        [Required]
        public string projectname{get;set;}

        [Required]
        public string date{get;set;}

        [Required]
        public int Totalhoursworked{get;set;}

        [Required]
        public string status{get;set;}

       //reference for relation with Task class

        public List<MyTask>tasks{get;set;} 



        //Method To send mail whether Timesheet is approved or not.

        public static void Acknowledgementmail(string mailid,int id,string status)
          {
            var smtpClient=new SmtpClient("smtp.gmail.com")
            {
                Port=587,
                Credentials=new NetworkCredential("kalaivani0720@gmail.com","mxctojbhpiurquoo"),
                EnableSsl=true,
            };
            if(status=="Denie")
            {
                var mailmessage=new MailMessage
                {
                    From=new MailAddress("kalaivani0720@gmail.com"),
                    Subject="Timesheet Acknowledgement",
                    Body="<h1>Timesheet id " + id + "is"+status+"</h1>",
                    IsBodyHtml=true,
                };
                mailmessage.To.Add(mailid);
                smtpClient.Send(mailmessage);

            }
            else
            {
                var mailmessage=new MailMessage
                {
                    From=new MailAddress("kalaivani0720@gmail.com"),
                    Subject="Timesheet Acknowledgement",
                    Body="<h1>Timesheet id " + id + "is"+status+"</h1>",
                    IsBodyHtml=true,
                };
                mailmessage.To.Add(mailid);
                smtpClient.Send(mailmessage);

            }
            
        }

    }
    public class MyTask
    {
        public Guid taskid{get;set;}

        [Required(ErrorMessage = "Enter Taskname")]
        public string taskname{get;set;}

        [Required(ErrorMessage = "Enter Username")]
        public string username{get;set;}

        [Required(ErrorMessage = "Enter start time")]
        public TimeSpan start_time{get;set;}

        [Required(ErrorMessage = "Enter End Time")]
        public TimeSpan end_time{get;set;}

        [Required(ErrorMessage = "Enter TimesheetId")]
        public  int TimesheetId{get;set;}

        [Required(ErrorMessage = "Enter Hoursworked")]
        public int hoursworked{get;set;}

        [Required(ErrorMessage = "Enter Taskstatus")]
        public string taskstatus{get;set;}

        //reference for relation with Timesheet class
        
        public Timesheet mytimesheet{get;set;}

    }
    
        

}