using Microsoft.AspNetCore.Mvc;
using Proyecto.ViewModels;
using Rotativa.AspNetCore;

namespace Proyecto.Areas.TutoresEmpresa.Controllers
{
    [Area("TutoresEmpresa")]
    public class EvaluacionFCTController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EvaluacionFCTViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.FechaFirma = DateTime.Now;

            var pdf = new ViewAsPdf("EvaluacionPDF", model);

            // ⬇️ await porque BuildFile devuelve Task<byte[]>
            var pdfBytes = await pdf.BuildFile(ControllerContext);

            var fileName = $"Evaluacion_{model.AlumnoNombre}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "evaluaciones");
            var filePath = Path.Combine(folderPath, fileName);

            Directory.CreateDirectory(folderPath);
            System.IO.File.WriteAllBytes(filePath, pdfBytes);

            return Redirect($"/evaluaciones/{fileName}");

        }
    }
}
