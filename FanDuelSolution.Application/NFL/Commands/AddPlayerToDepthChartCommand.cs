using FanDuelSolution.Application.Interfaces;
using FanDuelSolution.Application.NFL.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FanDuelSolution.Application.NFL.Commands;

public class AddPlayerToDepthChartCommand : IRequest<bool>
{
    public Player Player { get; set; } = new();

    //Per question, member is nullable
    public int? PositionDepth { get; set; }
}

public class AddPlayerToDepthChartCommandHandler : IRequestHandler<AddPlayerToDepthChartCommand, bool>
{
    private readonly ILogger<AddPlayerToDepthChartCommandHandler> _logger;
    private readonly IPlayerRepository _playerRepository;

    public AddPlayerToDepthChartCommandHandler(IPlayerRepository playerRepository, ILogger<AddPlayerToDepthChartCommandHandler> logger)
    {
        _playerRepository = playerRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(AddPlayerToDepthChartCommand command, CancellationToken cancellationToken)
    {
        try
        {
            _playerRepository.AddPlayerToDepthChart(command.Player, command.PositionDepth);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to AddPlayerToDepthChart for Player Number: {command.Player.Number}, Player Name: {command.Player?.Name}");

            return await Task.FromResult(false);
        }

        //Note: return type is not specified in the question
        return await Task.FromResult(true);
    }
}