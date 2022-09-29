using FMS.Data.Models;

namespace FMS.Web.Models
{   
    public class MedNoteSearchViewModel
    {
        // result set
        public IList<MedicalHistory> MedicalNotes { get; set;} = new List<MedicalHistory>();

        // search options        
        public string Query { get; set; } = "";
        public MedNoteRange MedNoteRange { get; set; } = MedNoteRange.ONGOING;
    }
}