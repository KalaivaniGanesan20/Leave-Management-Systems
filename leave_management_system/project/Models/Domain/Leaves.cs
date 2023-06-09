using System.ComponentModel.DataAnnotations;
using CustomValidation;

namespace leave.Models.Domain
{
    public class Leaves
    {
      [Key]
      public int leaveid{get;set;}

     [Required (ErrorMessage = "Enter Employeeid")]

      public int EmployeeId{get;set;}


      [Required (ErrorMessage = "Enter Leave type")]
      public string? leavetype{get;set;}


      [Required(ErrorMessage = "Enter Username")]
      public string? username{get;set;}


      [Required(ErrorMessage = "Enter UserRole")]
      public string? userRole{get;set;}


      [Required (ErrorMessage = "Enter Leavereason")]
      public string? leaveReason{get;set;}


      [Required(ErrorMessage = "Enter Fromdate")]
      public DateTime? leaveFrom{get;set;}
      

      [Required(ErrorMessage = "Enter Todate")]
      [FromDateToToDate]
      public DateTime? leaveTo{get;set;}
     
      public int Numberofdays{get;set;}

      public string? status{get;set;} 

    }
}