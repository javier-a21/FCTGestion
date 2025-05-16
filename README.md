# Gestión FCT - Aplicación web para centros educativos

Este proyecto es una aplicación web desarrollada con ASP.NET Core pensada para facilitar la gestión de la Formación en Centros de Trabajo (FCT) en los institutos. Su objetivo es simplificar y centralizar el proceso que realizan cada año los centros educativos, permitiendo un seguimiento más cómodo y ordenado del alumnado, tutores del centro y tutores de empresa.

## Características del proyecto

- Sistema de usuarios con roles diferenciados:
  - Administrador: gestión completa de datos y usuarios.
  - Tutor del centro: seguimiento del alumnado y registro de contactos con empresas.
  - Tutor de empresa: validación semanal de tareas del alumno.
  - Alumno: consulta de su asignación y registro diario de prácticas.

- Registro diario de tareas por el alumnado.
- Validación de esas tareas por parte del tutor de empresa.
- Gestión centralizada de empresas colaboradoras y sus tutores.
- Asignación de prácticas con fechas, horario y empresa.
- Separación del contenido por áreas según el tipo de usuario.

## Tecnologías utilizadas

- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server
- Razor Pages y MVC
- Identity para la autenticación
- Bootstrap 5
- JavaScript para funcionalidades dinámicas

## Despliegue de la Aplicación FCTGestion
Requisitos previos
-.NET 8 SDK

-Visual Studio 2022 o superior

-SQL Server LocalDB o SQL Server completo

-Git
--------------------------------------------------------
--------------------------------------------------------
Pasos para el despliegue: 

1. Clonar el Repositorio
#git clone URL del Proyecto de GitHub.
cd FCTGestion
 2. Crear el archivo appsettings.json
En la raíz del proyecto, crea un archivo appsettings.json con el siguiente contenido:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=FCTGestion;Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```
--------------------------------------------------------
Nota: Si usas SQL Server completo o SQLite, ajusta la cadena de conexión.

 3. Restaurar Paquetes
#dotnet restore

 5. Aplicar Migraciones
#dotnet ef database update

 6. Ejecutar la Aplicación
#dotnet run
Accede en https://localhost:5001 o http://localhost:5000.

 7. Usuarios de Prueba
Administrador: admin@example.com / Admin123!

Alumnos y tutores deben ser creados manualmente.
Contraseña por defecto:

-Tutores Centro: "TutorCentro123."

-Tutores Empresa: "TutorEmpresa123."
