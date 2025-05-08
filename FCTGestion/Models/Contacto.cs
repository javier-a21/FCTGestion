using System;
using System.ComponentModel.DataAnnotations;

namespace FCTGestion.Models
{
    public class Contacto
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "ID del TutorCentro")]
        public int TutorCentroId { get; set; }
        public virtual TutorCentro? TutorCentro { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha del contacto")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Alumno Contactado")]
        public int? AlumnoId { get; set; }
        public virtual Alumno? Alumno { get; set; }

        [Display(Name = "Tutor Empresa Contactado")]
        public int? TutorEmpresaId { get; set; }
        public virtual TutorEmpresa? TutorEmpresa { get; set; }

        [Display(Name = "Medio de contacto (teléfono, email...)")]
        public string? Medio { get; set; }

        [Display(Name = "Resumen de la conversación")]
        public string? Resumen { get; set; }
    }
}
