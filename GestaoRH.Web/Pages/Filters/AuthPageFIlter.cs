using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GestaoRH.Web.Filters
{
    public class AuthPageFilter : IPageFilter
    {
        public void OnPageHandlerSelected(PageHandlerSelectedContext context) { }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var path = context.HttpContext.Request.Path.Value;

            // Permitir acesso às páginas de conta
            if (path!.StartsWith("/Conta/Login", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/Conta/Register", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var isLoggedIn = context.HttpContext.Request.Cookies.ContainsKey("token");

            if (!isLoggedIn)
            {
                context.Result = new RedirectToPageResult("/Conta/Login");
            }
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context) { }
    }
}
