using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FCTGestion.Models
{
    public class Empresa
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public ICollection<TutorEmpresa> Tutores { get; set; }
        public ICollection<RAE> Relaciones { get; set; } = new List<RAE>();

    }
}
