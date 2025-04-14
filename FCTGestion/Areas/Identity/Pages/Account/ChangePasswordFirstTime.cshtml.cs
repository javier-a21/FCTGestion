using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FCTGestion.Areas.Identity.Pages.Account
{
    [Authorize]
    public class ChangePasswordFirstTimeModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ChangePasswordFirstTimeModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            user.DebeCambiarPassword = false;
            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            // Redirige según el rol
            if (await _userManager.IsInRoleAsync(user, "TutorCentro"))
                return RedirectToAction("Index", "PanelTutorCentro", new { area = "TutorCentro" });

            if (await _userManager.IsInRoleAsync(user, "TutorEmpresa"))
                return RedirectToAction("Index", "PanelTutorEmpresa", new { area = "TutorEmpresa" });

            if (await _userManager.IsInRoleAsync(user, "Alumno"))
                return RedirectToAction("Index", "PanelAlumno", new { area = "Alumnos" });

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Index", "PanelAdmin", new { area = "Admin" });

            return RedirectToPage("/Index");
        }
    }
}
