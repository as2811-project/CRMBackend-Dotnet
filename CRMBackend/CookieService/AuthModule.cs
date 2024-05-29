namespace CRMBackend.CookieService;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Supabase;

public static class CookieHelper
{
    public static async Task<(bool IsAuthenticated, string ErrorMessage)> Authenticate(HttpContext httpContext, Client client)
    {
        if (!httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader) ||
            !httpContext.Request.Headers.TryGetValue("X-Refresh-Token", out var refreshTokenHeader))
        {
            return (false, "Authorization or Refresh Token headers missing.");
        }

        var jwtToken = authorizationHeader.ToString().Split(' ').Last();
        var refreshToken = refreshTokenHeader.ToString();

        try
        {
            var session = await client.Auth.SetSession(jwtToken, refreshToken, false);

            if (session == null)
            {
                return (false, "Invalid session.");
            }

            return (true, "Success");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
