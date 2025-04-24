using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCTGestion.Models
{
    public class TutorCentro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }

        [Display(Name = "Usuario vinculado")]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        [Display(Name = "Cuenta de usuario")]
        public ApplicationUser? Usuario { get; set; }

        [Display(Name = "Alumnos asignados")]
        public ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();
    }
}
