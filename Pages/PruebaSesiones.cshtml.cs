using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ProyectoAlmacen.Pages
{
    public class PruebasModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public PruebasModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            //En las funciones OnGet de las paginas revisas que el tipo de usuario coincida y de no ser así rediriges a la pagina de error
            // if (HttpContext.Session.GetString("TipoUsuario") != "Almacenista")
            // {
            //     Response.Redirect("/Error");
            // }
        }

        public IActionResult OnPostLogout()
        {
            // Cerrar la sesión
            HttpContext.Session.Clear();

            // Redirigir a la página de inicio
            return RedirectToPage("/Index");
        }
    }
}
