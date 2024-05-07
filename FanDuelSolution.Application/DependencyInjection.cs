using FanDuelSolution.Application.NFL.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace FanDuelSolution.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //Add NFL, and other mediator config as required
        services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<AddPlayerToDepthChartCommand>());

        return services;
    }
}