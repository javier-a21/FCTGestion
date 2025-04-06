using System;
using System.ComponentModel.DataAnnotations;

namespace FCTGestion.Models
{
    public class Contacto
    {
        public int Id { get; set; }

        public int TutorCentroId { get; set; }
        public TutorCentro TutorCentro { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string ConQuien { get; set; }

        public string Como { get; set; }

        public string Resumen { get; set; }
    }
}
