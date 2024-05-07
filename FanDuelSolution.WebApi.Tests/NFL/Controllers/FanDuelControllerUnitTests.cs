using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FanDuelSolution.Application.NFL.Commands;
using FanDuelSolution.Application.NFL.Models;
using FanDuelSolution.Application.NFL.Queries;
using FanDuelSolution.WebApi.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FanDuelSolution.WebApi.Tests.NFL.Controllers;

public class FanDuelControllerUnitTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IMediator> _mockMediator;

    public FanDuelControllerUnitTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _mockMediator = _fixture.Freeze<Mock<IMediator>>();

    }

    [Theory, AutoData]
    public async Task GetFullDepthChartAsync_WithValidRequest_ReturnsExpectedResult(string expectedDepthChartResponse)
    {
        //arrange
        _mockMediator
            .Setup(c => c.Send(It.IsAny<GetFullDepthChartQuery>(), CancellationToken.None))
            .ReturnsAsync(expectedDepthChartResponse);

        var controller = new NFLDepthChartController(_mockMediator.Object);

        //act
        var result = await controller.GetFullDepthChartAsync() as OkObjectResult;

        //assert
        result?.Value.Should().BeEquivalentTo(expectedDepthChartResponse);
    }

    [Theory, AutoData]
    public async Task GetFullDepthChartAsync_WithException_ReturnsInternalServerError(Exception expectedException)
    {
        //arrange
        _mockMediator
            .Setup(c => c.Send(It.IsAny<GetFullDepthChartQuery>(), CancellationToken.None))
            .ThrowsAsync(expectedException);

        var controller = new NFLDepthChartController(_mockMediator.Object);

        //act
        var action = async () => await controller.GetFullDepthChartAsync();

        //assert
        await action.Should().ThrowAsync<Exception>();
    }

    [Theory, AutoData]
    public async Task GetPlayerBackupsFromDepthChartAsync_WithValidRequest_ReturnsExpectedResult(
        GetPlayerBackupsFromDepthChartQuery expectedQuery,
        List<Player> expectedPlayersResponse)
    {
        //arrange
        _mockMediator
            .Setup(c => c.Send(expectedQuery, CancellationToken.None))
            .ReturnsAsync(expectedPlayersResponse);

        var controller = new NFLDepthChartController(_mockMediator.Object);

        //act
        var result = await controller.GetPlayerBackupsFromDepthChartAsync(expectedQuery) as OkObjectResult;

        //assert
        result?.Value.Should().BeEquivalentTo(expectedPlayersResponse);
    }

    [Theory, AutoData]
    public async Task GetPlayerBackupsFromDepthChartAsync_WithException_ReturnsInternalServerError(
        GetPlayerBackupsFromDepthChartQuery expectedQuery,
        Exception expectedException)
    {
        //arrange
        _mockMediator
            .Setup(c => c.Send(expectedQuery, CancellationToken.None))
            .ThrowsAsync(expectedException);

        var controller = new NFLDepthChartController(_mockMediator.Object);

        //act
        var action = async () => await controller.GetPlayerBackupsFromDepthChartAsync(expectedQuery);

        //assert
        await action.Should().ThrowAsync<Exception>();
    }

    [Theory, AutoData]
    public async Task AddPlayerToDepthChartAsync_WithValidRequest_ReturnsExpectedResult(
        AddPlayerToDepthChartCommand expectedCommand,
        bool expectedResponse)
    {
        //arrange
        _mockMediator
            .Setup(c => c.Send(expectedCommand, CancellationToken.None))
            .ReturnsAsync(expectedResponse);

        var controller = new NFLDepthChartController(_mockMediator.Object);

        //act
        var result = await controller.AddPlayerToDepthChartAsync(expectedCommand) as OkObjectResult;

        //assert
        result?.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Theory, AutoData]
    public async Task AddPlayerToDepthChartAsync_WithException_ReturnsInternalServerError(
        AddPlayerToDepthChartCommand expectedCommand,
        Exception expectedException)
    {
        //arrange
        _mockMediator
            .Setup(c => c.Send(expectedCommand, CancellationToken.None))
            .ThrowsAsync(expectedException);

        var controller = new NFLDepthChartController(_mockMediator.Object);

        //act
        var action = async () => await controller.AddPlayerToDepthChartAsync(expectedCommand);

        //assert
        await action.Should().ThrowAsync<Exception>();
    }

    [Theory, AutoData]
    public async Task RemovePlayerFromDepthChartAsync_WithValidRequest_ReturnsExpectedResult(
        RemovePlayerDepthChartCommand expectedCommand,
        List<Player> expectedPlayers)
    {
        //arrange
        _mockMediator
            .Setup(c => c.Send(expectedCommand, CancellationToken.None))
            .ReturnsAsync(expectedPlayers);

        var controller = new NFLDepthChartController(_mockMediator.Object);

        //act
        var result = await controller.RemovePlayerFromDepthChartAsync(expectedCommand) as OkObjectResult;

        //assert
        result?.Value.Should().BeEquivalentTo(expectedPlayers);
    }

    [Theory, AutoData]
    public async Task RemovePlayerFromDepthChartAsync_WithException_ReturnsInternalServerError(
        RemovePlayerDepthChartCommand expectedCommand,
        Exception expectedException)
    {
        //arrange
        _mockMediator
            .Setup(c => c.Send(expectedCommand, CancellationToken.None))
            .ThrowsAsync(expectedException);

        var controller = new NFLDepthChartController(_mockMediator.Object);

        //act
        var action = async () => await controller.RemovePlayerFromDepthChartAsync(expectedCommand);

        //assert
        await action.Should().ThrowAsync<Exception>();
    }
}