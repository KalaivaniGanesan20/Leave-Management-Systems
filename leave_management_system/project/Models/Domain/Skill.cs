using System.ComponentModel.DataAnnotations;
#nullable disable
namespace EmployeeSkilldataModels

//web api Model
{
    public class Employees
    {
        [Key]
        public int Id{get;set;}

        [Required]
        public string name{get;set;}

        //Reference relating to skill class
        
        public List<Skill> skill{get;set;}
    }
    public class Skill
    {
        [Key]
        public int skillid{get;set;}

        [Required]
        public string skilltype{get;set;}

        [Required]
        public int Id{get;set;}  //Foreign key 
       
        [Required]
        public string specialization_name{get;set;}

        [Required]
        public string skilllevel{get;set;}
    }
}