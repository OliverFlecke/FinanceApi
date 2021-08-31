using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace FinanceApi
{
    static class HttpContextExtensions
    {
        public static int GetUserId(this HttpContext context)
        {
            var claim = context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            return int.Parse(claim.Value);
        }
    }
}
