using FMS.Data.Models;

namespace SMS.Web.Models
{   
    public class TicketSearchViewModel
    {
        // result set
        public IList<MedicalHistory> Tickets { get; set;} = new List<MedicalHistory>();

        // search options        
        public string Query { get; set; } = "";
        public TicketRange Range { get; set; } = TicketRange.OPEN;
    }
}