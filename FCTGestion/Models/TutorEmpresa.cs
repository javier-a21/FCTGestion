using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FCTGestion.Models
{
    public class TutorEmpresa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Debe asignarse una empresa.")]
        [Display(Name = "Empresa asignada")]
        public int? EmpresaId { get; set; }

        [ValidateNever]
        [ForeignKey("EmpresaId")]
        [Display(Name = "Empresa")]
        public Empresa? Empresa { get; set; }

        [Display(Name = "Usuario vinculado")]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        [Display(Name = "Cuenta de usuario")]
        public ApplicationUser? Usuario { get; set; }
    }
}
