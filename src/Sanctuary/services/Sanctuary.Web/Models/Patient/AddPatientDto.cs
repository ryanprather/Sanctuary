using System.ComponentModel.DataAnnotations;

namespace Sanctuary.Web.Models.Patient
{
    public class AddPatientDto
    {
        [Required]
        public string Identifier { get; set; }

        [Required]
        public string Site { get; set; }
    }
}
