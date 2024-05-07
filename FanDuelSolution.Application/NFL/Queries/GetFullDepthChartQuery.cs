using FanDuelSolution.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FanDuelSolution.Application.NFL.Queries;

public class GetFullDepthChartQuery : IRequest<string>
{
    //Empty
}

public class GetFullDepthChartQueryHandler : IRequestHandler<GetFullDepthChartQuery, string>
{
    private readonly ILogger<GetFullDepthChartQueryHandler> _logger;
    private readonly IPlayerRepository _playerRepository;

    public GetFullDepthChartQueryHandler(IPlayerRepository playerRepository, ILogger<GetFullDepthChartQueryHandler> logger)
    {
        _playerRepository = playerRepository;
        _logger = logger;
    }

    public async Task<string> Handle(GetFullDepthChartQuery query, CancellationToken cancellationToken)
    {
        var result = string.Empty;

        try
        {
            result = _playerRepository.GetFullDepthChart();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to handle GetFullDepthChartQuery");

            return await Task.FromResult(result);
        }

        return await Task.FromResult(result);
    }
}
