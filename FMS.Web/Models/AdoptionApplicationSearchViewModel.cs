using FMS.Data.Models;

namespace FMS.Web.Models
{   
    public class AdoptionApplicationSearchViewModel
    {

        // result set
        public IList<AdoptionApplication> AdoptionApplications { get; set;} = new List<AdoptionApplication>();

        // search options        
        public string Query { get; set; } = "";
        public AdoptionApplicationRange AdoptionApplicationRange { get; set; } = AdoptionApplicationRange.AWAITING;
    }
}




