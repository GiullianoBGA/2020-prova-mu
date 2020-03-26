using Microsoft.AspNetCore.Components.Authorization;
using Prova.Modelos;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Prova.Frontend.Helpers
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly TokenViewModel tokenViewModel;

        public CustomAuthStateProvider(TokenViewModel tokenViewModel)
        {
            this.tokenViewModel = tokenViewModel;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity = new ClaimsIdentity();
            ClaimsPrincipal user = new ClaimsPrincipal(identity);

            if (tokenViewModel == null || string.IsNullOrEmpty(tokenViewModel.Login))
            {
                return Task.FromResult(new AuthenticationState(user));
            }
            else
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, tokenViewModel.Login),
                }, "JWT authentication type");

                user = new ClaimsPrincipal(identity);

                base.NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

                return Task.FromResult(new AuthenticationState(user));
            }
        }

        public Task<AuthenticationState> Logoff()
        {
            tokenViewModel.Logged = false;
            tokenViewModel.Login = string.Empty;
            tokenViewModel.Token = string.Empty;

            ClaimsIdentity identity = new ClaimsIdentity();
            ClaimsPrincipal user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }
    }
}
