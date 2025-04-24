using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FCTGestion.Models
{
    public class Empresa
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre de la empresa")]
        public string Nombre { get; set; }

        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Display(Name = "Tutores de empresa")]
        public ICollection<TutorEmpresa> Tutores { get; set; } = new List<TutorEmpresa>();
    }
}
