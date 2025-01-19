using System.ComponentModel.DataAnnotations;

namespace Sanctuary.Web.Models.Patient
{
    public class EditPatientDto
    {
        public Guid Id { get; set; }

        [Required]
        public required string Identifier { get; set; }
    }
}
