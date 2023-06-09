using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EmployeedataModels
{

    public class Employee
    {
        [Display(Name="Employee Id")]
        [Required(ErrorMessage = "Enter UserId")]
        [RegularExpression(@"\d{4,10}")]
        //[Remote("CheckId","Employee")]
        public int Id{get;set;}

        [Display(Name ="User Name")]
        [Required(ErrorMessage = "Enter Username")]
        [StringLength(15,MinimumLength =3)]
       // [Remote("Checkusername","Employee")]
        [RegularExpression(@"^[a-z]+[0-9]+\S$",ErrorMessage="Enter properusername => only lowecase => only numbers => NO Whitespace")]
        public string? username{get;set;}

        [Display(Name ="First Name")]
        [Required(ErrorMessage = "Enter Firstname")]
        [StringLength(15,MinimumLength =3)]
        [RegularExpression(@"^[A-Z][a-z]+\S$",ErrorMessage="Enter proper Firstname start with captial letter only alphabets")]//+ one or more * zero or more ? zero or one
        public string? Firstname{get;set;}

        [Display(Name ="Last Name")]
        [Required(ErrorMessage = "Enter Lastname")]
        [StringLength(15,MinimumLength =1)]
        [RegularExpression(@"^[A-Za-z]+$",ErrorMessage="Enter proper Lastname only Alphabets")]
        public string? Lastname{get;set;}

       
        [Display(Name ="Date Of Birth")]
        [DisplayFormat(DataFormatString ="{0:d}")]
        [Required(ErrorMessage = "Enter Date of Birth")]
        //[RegularExpression(@"^(0[1-9]|1[0-2])/(0[1-9]|1\d|2[0-9]|3[01])/2[0-9]{3}$",ErrorMessage ="enter proper date")]
        //Month 01-12 date 01-31 year 2000-2999
        public DateTime dateofbirth{get;set;}

        [Display(Name ="Email Id")]
        [Required(ErrorMessage = "Enter Emailid")]
        [RegularExpression(@"[a-z][a-z0-9]+[@]+[a-z]{2,5}[.]+[a-z]{1,3}\S",ErrorMessage="enter proper mail id =>No other special characters allowed =>only lowercase and numbers")]
        [DataType(DataType.EmailAddress)]
        public string? emailid{get;set;}
        
        [Display(Name ="Password")]
        [Required(ErrorMessage = "Enter Password")]
        [StringLength(10,MinimumLength =8,ErrorMessage ="Minimum length is 8")]
        [RegularExpression(@"^(?=.*[\d])(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*])[\w!@#$%^&*]{6,16}$",ErrorMessage ="atleast one uppercase")]
        public string? userpassword{get;set;}

        [Display(Name ="Phone Number")]
        [Required(ErrorMessage = "Enter contact_number")]
        [RegularExpression(@"^[6-9]\d{9}$",ErrorMessage ="Enter proper contact_number")]
        [DataType(DataType.PhoneNumber)]
        public string? contact_number{get;set;}

        [Display(Name ="Date Of Joined")]
        [DisplayFormat(DataFormatString ="{0:d}")]
        [Required(ErrorMessage = "Enter Date of Joined")]
        //[RegularExpression(@"^(0[1-9]|1[0-2])/(0[1-9]|1\d|2[0-9]|3[01])/2[0-9]{3}$",ErrorMessage ="enter proper date")]
        //Month 01-12 date 01-31 year 2000-2999
        public DateTime dateofjoined{get;set;}

        [Display(Name ="Department")]
        [Required(ErrorMessage = "Enter Department")]
         public string? department{get;set;}
         
        [Display(Name ="HR Name")]
        [Required(ErrorMessage = "Enter HRname")]
         public string? HR{get;set;}
        
        [Display(Name="Manager Name")]
        [Required(ErrorMessage = "Enter Managername")]

         public string? Manager{get;set;}

        [Display(Name ="Salary")]
        [DisplayFormat(DataFormatString ="{0:C}")]
        [Required(ErrorMessage = "Enter Salary")]
        //[HiddenInput(DisplayValue =true)]
        public int salary{get;set;}
        public string? searchvalue{get;set;}
      
        [Display(Name ="Photo")]
        public string? ImageFile{get;set;}

        [NotMapped]
        public IFormFile? ImageUrl { get; set; }

    }
}

 // [Display(Name ="Age")]
        // [Required(ErrorMessage = "Enter proper Age")] 
        // [Range(21,55, ErrorMessage = "Age must be between 21 and 55")]
        // public int age{get;set;}