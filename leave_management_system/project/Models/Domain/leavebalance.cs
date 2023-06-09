using System.ComponentModel.DataAnnotations;
#nullable disable
namespace leavebalanceModel
{
    public class leavebalance
    {
        [Key]
        public int Balanceid{get;set;}
        
        [Required]
        public int Employeeid{get;set;}

        [Required]
         public string username{get;set;}

         [Required]
         public int sickleave{get;set;}

         [Required]
         public int Compensatoryleave{get;set;}

         [Required]
         public int casualleave{get;set;}

         [Required]
         public int Lossofpay{get;set;}

         [Required]
         public int annualleave{get;set;}
    }
}