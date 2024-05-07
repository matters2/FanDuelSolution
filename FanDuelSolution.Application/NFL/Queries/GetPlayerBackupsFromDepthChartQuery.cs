using FanDuelSolution.Application.Interfaces;
using FanDuelSolution.Application.NFL.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FanDuelSolution.Application.NFL.Queries;

public class GetPlayerBackupsFromDepthChartQuery : IRequest<List<Player>>
{
    public string Position { get; set; } = string.Empty;
    public Player Player { get; set; } = new();
}

public class GetPlayerBackupsFromDepthChartQueryHandler : IRequestHandler<GetPlayerBackupsFromDepthChartQuery, List<Player>>
{
    private readonly ILogger<GetPlayerBackupsFromDepthChartQueryHandler> _logger;
    private readonly IPlayerRepository _playerRepository;

    public GetPlayerBackupsFromDepthChartQueryHandler(IPlayerRepository playerRepository, ILogger<GetPlayerBackupsFromDepthChartQueryHandler> logger)
    {
        _playerRepository = playerRepository;
        _logger = logger;
    }

    public async Task<List<Player>> Handle(GetPlayerBackupsFromDepthChartQuery query, CancellationToken cancellationToken)
    {
        var result = new List<Player>();

        try
        {
            result = _playerRepository.GetBackups(query.Position, query.Player);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to GetPlayerBackupsFromDepthChartQuery for Player Number: {query.Player.Number}, Player Name: {query.Player?.Name}");

            return await Task.FromResult(result);
        }

        //Note: return type is not specified in the question
        return await Task.FromResult(result);
    }
}