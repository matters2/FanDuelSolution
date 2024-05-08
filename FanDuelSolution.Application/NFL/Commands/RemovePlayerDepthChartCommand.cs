using FanDuelSolution.Application.Interfaces;
using FanDuelSolution.Application.NFL.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FanDuelSolution.Application.NFL.Commands;

public class RemovePlayerDepthChartCommand : IRequest<List<Player>>
{
    public Player Player { get; set; } = new();
}

public class RemovePlayerDepthChartCommandHandler : IRequestHandler<RemovePlayerDepthChartCommand, List<Player>>
{
    private readonly ILogger<RemovePlayerDepthChartCommandHandler> _logger;
    private readonly IPlayerRepository _playerRepository;

    public RemovePlayerDepthChartCommandHandler(IPlayerRepository playerRepository, ILogger<RemovePlayerDepthChartCommandHandler> logger)
    {
        _playerRepository = playerRepository;
        _logger = logger;
    }

    public async Task<List<Player>> Handle(RemovePlayerDepthChartCommand command, CancellationToken cancellationToken)
    {
        var result = new List<Player>();

        try
        {
            result = _playerRepository.RemovePlayerFromDepthChart(command.Player);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to RemovePlayerDepthChartCommand for Player Number: {command.Player?.Number}, Player Name: {command.Player?.Name}");

            return await Task.FromResult(result);
        }

        return await Task.FromResult(result);
    }
}