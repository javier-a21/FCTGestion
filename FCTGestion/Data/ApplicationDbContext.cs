using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FCTGestion.Models;

namespace FCTGestion.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Alumno> Alumnos { get; set; } = default!;
        public DbSet<TutorCentro> TutoresCentro { get; set; } = default!;
        public DbSet<Empresa> Empresas { get; set; } = default!;
        public DbSet<TutorEmpresa> TutoresEmpresa { get; set; } = default!;
        public DbSet<RAE> RAEs { get; set; } = default!;
        public DbSet<TareaDiaria> TareasDiarias { get; set; } = default!;
        public DbSet<Contacto> Contactos { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RAE>()
                .HasOne(r => r.Alumno)
                .WithMany()
                .HasForeignKey(r => r.AlumnoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RAE>()
                .HasOne(r => r.Empresa)
                .WithMany(e => e.Relaciones)
                .HasForeignKey(r => r.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RAE>()
                .HasOne(r => r.TutorEmpresa)
                .WithMany(te => te.Relaciones)
                .HasForeignKey(r => r.TutorEmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        }





    }

}
