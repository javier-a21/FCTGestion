namespace FCTGestion.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
    public class Alumno
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Correo educativo")]
        public string CorreoEducacion { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Número Seguridad Social")]
        public string SeguridadSocial { get; set; } = string.Empty;

   
        [Display(Name = "Usuario vinculado")]
        public string? UserId { get; set; }

        [Display(Name = "Tutor del centro")]
        public int? TutorCentroId { get; set; }

        public TutorCentro? TutorCentro { get; set; }
        [Display(Name = "Empresa asignada")]
        public int? EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa? Empresa { get; set; }

        [Display(Name = "Tutor de empresa")]
        public int? TutorEmpresaId { get; set; }

        [ForeignKey("TutorEmpresaId")]
        public TutorEmpresa? TutorEmpresa { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha inicio FCT")]
        public DateTime? FechaInicioFCT { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha fin FCT")]
        public DateTime? FechaFinFCT { get; set; }
}

