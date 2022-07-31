using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FMS.Data.Models
{   
    public enum TicketRange {OPEN,CLOSED,ALL}
    public class MedicalHistory
    {
        public int Id { get; set; }
        
        // suitable medical history attributes / relationships are detailed here
        [Required]
        [StringLength(300, MinimumLength = 5)]
        public string Report {get;set;}
        
        [Required]
        public string Medication {get;set;}

        [StringLength(500)]
        public string Resolution {get; set;}
        
        public DateTime CreatedOn {get; set;} = DateTime.Now;
        public DateTime ResolvedOn {get; set;} = DateTime.MinValue;

        public bool Active {get; set;} = true;         
        

        //Medical history note owned by a dog
        public int DogId {get;set;} //foreign key
        
        [JsonIgnore]
        public Dog Dog {get; set;}  //navigation property


    }
}
