using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCTGestion.Models
{
    public class TutorCentro
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Correo { get; set; }

        public string? UserId { get; set; }


        [ForeignKey("UserId")]
        public ApplicationUser? Usuario { get; set; }

        public ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();
    }
}
