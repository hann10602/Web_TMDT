using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoeStore.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Clear the session
            HttpContext.Session.Clear();

            // Redirect to the login page or homepage
            return RedirectToPage("/Auth/Login");
        }
    }
}
