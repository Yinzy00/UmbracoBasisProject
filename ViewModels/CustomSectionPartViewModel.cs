using System;
using System.ComponentModel.DataAnnotations;

namespace BasisProjectUmbraco.ViewModels
{
    public class CustomSectionPartViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Created { get; set; }
    }
}
