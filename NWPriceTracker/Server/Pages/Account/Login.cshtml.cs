using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace NWPriceTracker.Server.Pages.Authentication;

[AllowAnonymous]
public class LoginModel : PageModel
{
    public IActionResult OnGetAsync(string returnUrl = null)
    {
        // Request a redirect to the external login provider.
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Page("./login",
            pageHandler: "Callback",
            values: new { returnUrl }),
        };
        return new ChallengeResult("Discord", authenticationProperties);
    }

    public async Task<IActionResult> OnGetCallbackAsync(
        string returnUrl = null, string remoteError = null)
    {
        // Get the information about the user from the external login provider
        var discordUser = this.User.Identities.FirstOrDefault();
        if (discordUser.IsAuthenticated)
        {
            // Get email address
            string name = discordUser.FindFirst(ClaimTypes.Name).Value + '#' + 
                discordUser.Claims.Where(x => x.Type == "urn:discord:user:discriminator").First().Value;
            string pfpUrl = discordUser.Claims.Where(x => x.Type == "urn:discord:avatar:url").First().Value;

            // Throw error for unauthorized accounts
            if (!Account.IsAuthorized(name))
            {
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
                return Unauthorized();
            }
            
            // Authenticate as discord user
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = this.Request.Host.Value
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(discordUser),
                authProperties);
        }
        return LocalRedirect("/");
    }
}