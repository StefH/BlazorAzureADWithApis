using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAzureADWithApis.Client
{
    public class Program
    {
        public const string ServiceApiUrl = "https://localhost:44324";

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("BlazorAzureADWithApis.ServerAPI")
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { ServiceApiUrl },
                            scopes: new[] { "api://61b557d0-0018-4f72-8a7d-6145a941962d/Application.Access" }
                        );

                    return handler;
                });

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorAzureADWithApis.ServerAPI"));

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);

                // Add the 'delegated' scope
                options.ProviderOptions.DefaultAccessTokenScopes.Add($"api://{options.ProviderOptions.Authentication.ClientId}/access_as_user");
            });

            await builder.Build().RunAsync();
        }
    }
}