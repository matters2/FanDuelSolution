using AutoFixture;
using AutoFixture.AutoMoq;
using FanDuelSolution.Application.Interfaces;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using AutoFixture.Xunit2;
using FluentAssertions;
using FanDuelSolution.Application.NFL.Models;
using FanDuelSolution.Application.NFL.Queries;

namespace FanDuelSolution.Application.Tests.NFL.Queries;

public class GetPlayerBacksupsFromDepthChartQueryHandlerUnitTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IPlayerRepository> _mockPlayerRepository;
    private readonly Mock<ILogger<GetPlayerBackupsFromDepthChartQueryHandler>> _mockLogger;

    public GetPlayerBacksupsFromDepthChartQueryHandlerUnitTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _mockPlayerRepository = _fixture.Freeze<Mock<IPlayerRepository>>();
        _mockLogger = _fixture.Freeze<Mock<ILogger<GetPlayerBackupsFromDepthChartQueryHandler>>>();
    }

    [Theory, AutoData]
    public async Task Handle_GetPlayerBackupsFromDepthChartQuery_WithValidQuery_ReturnsExpectedResults(
        GetPlayerBackupsFromDepthChartQuery expectedQuery,
        List<Player> expectedResponse
        )
    {
        //arrange
        _mockPlayerRepository
            .Setup(c => c.GetBackups(expectedQuery.Player))
            .Returns(expectedResponse);

        var handler = new GetPlayerBackupsFromDepthChartQueryHandler(_mockPlayerRepository.Object, _mockLogger.Object);

        //act
        var result = await handler.Handle(expectedQuery, CancellationToken.None);

        //assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Theory, AutoData]
    public async Task Handle_GetPlayerBackupsFromDepthChartQuery_WithException_ReturnsEmptyListResult(
        GetPlayerBackupsFromDepthChartQuery expectedQuery,
        Exception expectedException
        )
    {
        //arrange
        _mockPlayerRepository
            .Setup(c => c.GetBackups(expectedQuery.Player))
            .Throws(expectedException);

        var handler = new GetPlayerBackupsFromDepthChartQueryHandler(_mockPlayerRepository.Object, _mockLogger.Object);

        //act
        var result = await handler.Handle(expectedQuery, CancellationToken.None);

        //assert
        result.Should().BeEmpty();
    }

    [Theory, AutoData]
    public async Task Handle_GetPlayerBackupsFromDepthChartQuery_WithException_LogsCorrectly(
        GetPlayerBackupsFromDepthChartQuery expectedQuery,
        Exception expectedException
        )
    {
        //arrange
        var expectedMessage = $"Failed to GetPlayerBackupsFromDepthChartQuery for Player Number: {expectedQuery.Player.Number}, Player Name: {expectedQuery.Player?.Name}";

        _mockPlayerRepository
            .Setup(c => c.GetBackups(expectedQuery.Player))
            .Throws(expectedException);

        var handler = new GetPlayerBackupsFromDepthChartQueryHandler(_mockPlayerRepository.Object, _mockLogger.Object);

        //act
        await handler.Handle(expectedQuery, CancellationToken.None);

        //assert
        _mockLogger.Verify(l => l.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
            It.Is<EventId>(eventId => eventId.Id == 0),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == expectedMessage && @type.Name == "FormattedLogValues"),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
        Times.Once);
    }
}