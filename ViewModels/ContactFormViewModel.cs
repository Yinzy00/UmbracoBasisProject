using System.ComponentModel.DataAnnotations;

namespace BasisProjectUmbraco.ViewModels
{
    public class ContactFormViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Bericht { get; set; }
    }
}
