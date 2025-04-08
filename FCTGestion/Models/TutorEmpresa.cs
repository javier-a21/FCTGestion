using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FCTGestion.Models
{
    public class TutorEmpresa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Debe asignarse una empresa.")]

        public int EmpresaId { get; set; }

        // Relación: un tutor pertenece a una empresa
        [ValidateNever]
        public Empresa Empresa { get; set; }

        // Relación con RAE (si la mantienes)
        public ICollection<RAE> Relaciones { get; set; } = new List<RAE>();
    }
}
