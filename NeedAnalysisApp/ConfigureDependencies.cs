using NeedAnalysisApp.Client.Repositories.Interfaces;
using NeedAnalysisApp.Client.Repositories.Services;
using NeedAnalysisApp.Data;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Repositories.Services;

namespace NeedAnalysisApp;

public static class ConfigureDependencies
{
    public static IServiceCollection InjectDependencies(this IServiceCollection services) 
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<ApplicationDbInitializer>();
        services.AddScoped<IIndustryService, IndustryService>();
        services.AddScoped<IAssessmentService, AssessmentService>(); 
        services.AddScoped<ILookUpService, LookUpService>(); 
        services.AddScoped<IQuestionService, QuestionService>(); 
        services.AddScoped<IOptionService, OptionService>();
        services.AddScoped<IUserService, UserService>();

        //services.AddTransient<HttpClient>();

        //services.AddScoped<IIndustryClientService, IndustryClientService>();
        //services.AddScoped<IAssessmentClientService, AssessmentClientService>();
        //services.AddScoped<IQuestionClientService, QuestionClientService>();
        return services;
    }
}
