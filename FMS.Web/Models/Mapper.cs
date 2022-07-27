using FMS.Data.Models;

namespace FMS.Web.Models
{
    public static class Mapper    
    {
        public static object ToDto(this MedicalHistory t)
        {
             return new {   
                Id = t.Id,
                Report = t.Report, 
                CreatedOn = t.CreatedOn,
                Active = t.Active,
                Resolution = t.Resolution,
                Dog = t.Dog?.Name
            };
        }
    }
}