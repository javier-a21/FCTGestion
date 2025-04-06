using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FCTGestion.Models
{
    public class TutorEmpresa
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        public ICollection<RAE> Relaciones { get; set; } = new List<RAE>();

    }
}
