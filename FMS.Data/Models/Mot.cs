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

        public DateOnly On {get; set;}

        public string MotTester {get;set;}

        public double Mileage {get; set;}


        //MOT owned by a user
        public int VehicleId {get;set;} //foreign key

        public Vehicle Vehicle {get; set;}  //navigation property


    }
}
