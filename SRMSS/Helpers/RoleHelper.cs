using Microsoft.AspNetCore.Mvc;

namespace SRMSS.Helpers
{
    public static class RoleHelper
    {
        public static IActionResult CheckRole(HttpContext context,
            params string[] roles)
        {
            string role = context.Session.GetString("Role");

            if (string.IsNullOrEmpty(role))
            {
                return new RedirectToActionResult(
                    "Login", "Login", null);
            }

            if (!roles.Contains(role))
            {
                return new RedirectToActionResult(
                    "AccessDenied", "Home", null);
            }

            return null;
        }
    }
}