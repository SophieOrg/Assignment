using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FMS.Data.Validators; //allows access to URL resource

namespace FMS.Data.Models
{   
    public class Sponsorship
    {
        public int Id { get; set; }

        public string TypeOfSponsorship {get; set;}    
        

        //Sponsorship owned by a dog
        public int DogId {get;set;} //foreign key
        
        [JsonIgnore]
        public Dog Dog {get; set;}  //navigation property


    }
}