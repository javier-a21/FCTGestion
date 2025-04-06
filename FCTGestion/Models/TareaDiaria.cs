using System;
using System.ComponentModel.DataAnnotations;

namespace FCTGestion.Models
{
    public class TareaDiaria
    {
        public int Id { get; set; }

        public int AlumnoId { get; set; }
        public Alumno Alumno { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public int Horas { get; set; }

        public string EstadoValidacion { get; set; } // Pendiente, Aprobada, Rechazada
    }
}
