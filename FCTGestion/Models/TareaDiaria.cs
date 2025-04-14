using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCTGestion.Models
{
    public enum EstadoValidacion
    {
        Pendiente,
        Aprobada,
        Rechazada
    }

    public class TareaDiaria
    {
        public int Id { get; set; }

        [Required]
        public int AlumnoId { get; set; }

        [ForeignKey("AlumnoId")]
        public Alumno Alumno { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de tarea")]
        public DateTime Fecha { get; set; }

        [Required]
        [Display(Name = "Descripcion tareas realizadas")]
        public string Descripcion { get; set; }

        [Required]
        [Range(1, 8, ErrorMessage = "Las horas deben estar entre 1 y 8.")]
        public int Horas { get; set; }

        [Required]
        [Display(Name = "Estado de validación")]
        public EstadoValidacion Estado { get; set; } = EstadoValidacion.Pendiente;

        [Display(Name = "Fecha de validación")]
        public DateTime? FechaValidacion { get; set; }

        [Display(Name = "Observaciones del tutor")]
        [StringLength(300)]
        public string? Observaciones { get; set; }
    }
}
