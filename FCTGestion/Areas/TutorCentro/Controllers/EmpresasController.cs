using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FCTGestion.Data;
using FCTGestion.Models;

namespace FCTGestion.Areas.TutorCentro.Controllers
{
    [Area("TutorCentro")]
    [Authorize(Roles = "TutorCentro")]
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpresasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var empresas = await _context.Empresas.ToListAsync();
            return View(empresas); // Se carga la vista Index.cshtml
        }
    }
}
