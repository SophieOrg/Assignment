using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FMS.Data.Models
{   
    //used in MOT search feature
    public enum Status {PASS,FAIL}
    public class Mot
    {
        public int Id { get; set; }
        
        // suitable mot attributes / relationships
        [Required]
        [StringLength(500, MinimumLength = 5)]
        public string Report {get;set;}
        
        [StringLength(500)]
        public string Resolution {get; set;}

        public DateTime CreatedOn {get; set;}= DateTime.Now;

        public DateTime ResolvedOn {get; set;} = DateTime.MinValue;

        public bool Active {get; set;} = true;

        //MOT owned by a user
        public int VehicleId {get;set;} //foreign key

        public Vehicle Vehicle {get; set;}  //navigation property


    }
}
