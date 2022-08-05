using FMS.Data.Models;

namespace FMS.Web.Models
{
    public static class AdoptionMapper    
    {
        public static object ToDto(this AdoptionApplication t)
        {
             return new {   
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber,
                Information = t.Information,
                Active = t.Active,
                Resolution = t.Resolution,
                Dog = t.Dog?.Name
            };
        }
    }
}