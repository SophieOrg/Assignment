using System;
using System.Collections.Generic;
	
using FMS.Data.Models;
	
namespace FMS.Data.Services
{
    // This interface describes the operations that a FleetService class should implement
    public interface IFleetService
    {
        void Initialise();
        
        // add suitable method definitions to implement assignment requirements            
    

        // ------------- User Management -------------------
        User Authenticate(string email, string password);
        User Register(string name, string email, string password, Role role);
        User GetUserByEmail(string email);
    
    }
    
}