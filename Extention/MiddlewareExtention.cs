using CrispTv.Interfaces;
using CrispTv.Services;

namespace CrispTv.Extention;

public static class MiddlewareExtention
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IPayStackService, PayStackService>();
        services.AddTransient<IFlutterWaveService, FlutterWaveService>();
    }
}
