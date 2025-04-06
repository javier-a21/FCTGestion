namespace FCTGestion.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


public class TutorCentro
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; }

    [Required]
    public string Correo { get; set; }

    public ICollection<Alumno> Alumnos { get; set; }
}

