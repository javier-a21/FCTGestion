using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.ViewModels
{
    public class EvaluacionFCTViewModel
    {
        // Datos generales
        [Required(ErrorMessage = "El nombre del alumno es obligatorio.")]
        public string AlumnoNombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de la empresa es obligatorio.")]
        public string EmpresaNombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ciclo formativo es obligatorio.")]
        public string CicloFormativo { get; set; } = string.Empty;

        [Range(1, 1000, ErrorMessage = "Las horas realizadas deben estar entre 1 y 1000.")]
        public int HorasRealizadas { get; set; }

        // Actitud
        [Required(ErrorMessage = "Debe seleccionar una opción para Asistencia.")]
        public string Asistencia { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una opción para Puntualidad.")]
        public string Puntualidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una opción para Responsabilidad.")]
        public string Responsabilidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una opción para Iniciativa.")]
        public string Iniciativa { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una opción para Trabajo en equipo.")]
        public string TrabajoEnEquipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una opción para Interés por las prácticas.")]
        public string InteresPracticas { get; set; } = string.Empty;

        // Aprendizaje y habilidades
        [Required(ErrorMessage = "Debe seleccionar una opción para Conocimientos previos.")]
        public string ConocimientosPrevios { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una opción para Habilidades prácticas.")]
        public string HabilidadesPracticas { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una opción para Organización y planificación.")]
        public string OrganizacionPlanificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una opción para Ritmo de trabajo.")]
        public string RitmoTrabajo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una opción para Desarrollo de tareas.")]
        public string DesarrolloTareas { get; set; } = string.Empty;

        // Resultados de aprendizaje
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos un resultado de aprendizaje.")]
        public List<int> ResultadosAprendizaje { get; set; } = new List<int>();

        // Evaluación final
        public bool Apto { get; set; }

        [MaxLength(1000, ErrorMessage = "Las observaciones no pueden superar los 1000 caracteres.")]
        public string Observaciones { get; set; } = string.Empty;

        public DateTime FechaFirma { get; set; } = DateTime.Now;
    }
}
