using FanDuelSolution.Application.Interfaces;
using FanDuelSolution.Infrastructure.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace FanDuelSolution.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IPlayerRepository, PlayerRepository>();

        return services;
    }
}