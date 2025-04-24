using System;
using System.ComponentModel.DataAnnotations;

namespace FCTGestion.Models
{
    public class Contacto
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tutor del Centro")]
        public int TutorCentroId { get; set; }

        public TutorCentro TutorCentro { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha del contacto")]
        public DateTime Fecha { get; set; }

        [Required]
        [Display(Name = "Con quién se contactó")]
        public string ConQuien { get; set; }

        [Display(Name = "Medio de contacto (teléfono, email...)")]
        public string? Como { get; set; }

        [Display(Name = "Resumen de la conversación")]
        public string? Resumen { get; set; }
    }
}
