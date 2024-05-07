using FanDuelSolution.Application.NFL.Commands;
using FanDuelSolution.Application.NFL.Models;
using FanDuelSolution.Application.NFL.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FanDuelSolution.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class NFLDepthChartController : ControllerBase
{
    private readonly IMediator _mediator;

    public NFLDepthChartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetFullDepthChart")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetFullDepthChartAsync()
    {
        var response = await _mediator.Send(new GetFullDepthChartQuery());
        return Ok(response);
    }

    [HttpGet("GetPlayerBackupsFromDepthChart")]
    [ProducesResponseType(typeof(List<Player>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPlayerBackupsFromDepthChartAsync([FromBody] GetPlayerBackupsFromDepthChartQuery query)
    {
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpPost("AddPlayerToDepthChart")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddPlayerToDepthChartAsync([FromBody] AddPlayerToDepthChartCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("RemovePlayerFromDepthChart")]
    [ProducesResponseType(typeof(List<Player>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> RemovePlayerFromDepthChartAsync([FromBody] RemovePlayerDepthChartCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}