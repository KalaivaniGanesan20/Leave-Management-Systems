using System.Net.Mail;
using System.Net;
namespace Userlogin
{
    public abstract class LogBase
    {
        /* abstract class */
        public abstract void log(string message);
    }
    public class MyLogger:LogBase
    {
        private string Currentdirectory{get;set;}
        private string Filename{get;set;}
        private string Filepath{get;set;}
        public MyLogger()
        {
            this.Currentdirectory=Directory.GetCurrentDirectory();
            this.Filename="Log.txt";
            this.Filepath=this.Currentdirectory+"/"+this.Filename;
        }
        public override void log(string message)
        {
            //write to file
            using(StreamWriter writer=File.AppendText(this.Filepath))
            {
                writer.Write("\n Log Entry : ");
                writer.Write("{0} {1}",DateTime.Now.ToLongTimeString(),DateTime.Now.ToLongDateString());
                writer.Write("username : {0} ",message);
                writer.Write("\n------------------------------------------------------ ");

            }
        }

    }
    public class Login
    {
    
        //Forgot password 
          
        public static void resetmail(string mailid)
          {
            var smtpClient=new SmtpClient("smtp.gmail.com")
            {
                Port=587,
                Credentials=new NetworkCredential("kalaivani0720@gmail.com","mxctojbhpiurquoo"),
                EnableSsl=true,
            };
            var mailmessage=new MailMessage
            {
                From=new MailAddress("kalaivani0720@gmail.com"),
                Subject="testmail verification",
                Body="<h1>hello</h1>",
                IsBodyHtml=true,
            };
            mailmessage.To.Add(mailid);
            smtpClient.Send(mailmessage);
        }
        
    }

}