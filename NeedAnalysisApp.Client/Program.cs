using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using NeedAnalysisApp.Client;
using NeedAnalysisApp.Client.Repositories.Interfaces;
using NeedAnalysisApp.Client.Repositories.Services;
internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddTransient<HttpClient>();

        builder.Services.AddMudServices();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
        builder.Services.AddScoped<IIndustryClientService, IndustryClientService>();
        builder.Services.AddScoped<IAssessmentClientService, AssessmentClientService>();
        builder.Services.AddScoped<IQuestionClientService, QuestionClientService>();
        builder.Services.AddScoped<IUserClientService, UserClientService>();

        await builder.Build().RunAsync();
    }
}