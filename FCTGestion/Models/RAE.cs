using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCTGestion.Models
{
    public class RAE
    {
        public int Id { get; set; }

        public int AlumnoId { get; set; }

        [ForeignKey("AlumnoId")]
        public Alumno Alumno { get; set; } = null!;

        public int EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; } = null!;

        public int TutorEmpresaId { get; set; }

        [ForeignKey("TutorEmpresaId")]
        public TutorEmpresa TutorEmpresa { get; set; } = null!;

        public string Horario { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
