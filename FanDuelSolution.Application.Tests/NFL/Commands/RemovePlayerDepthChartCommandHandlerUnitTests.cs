using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FanDuelSolution.Application.Interfaces;
using FanDuelSolution.Application.NFL.Commands;
using FanDuelSolution.Application.NFL.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FanDuelSolution.Application.Tests.NFL.Commands;

public class RemovePlayerDepthChartCommandHandlerUnitTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IPlayerRepository> _mockPlayerRepository;
    private readonly Mock<ILogger<RemovePlayerDepthChartCommandHandler>> _mockLogger;

    public RemovePlayerDepthChartCommandHandlerUnitTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _mockPlayerRepository = _fixture.Freeze<Mock<IPlayerRepository>>();
        _mockLogger = _fixture.Freeze<Mock<ILogger<RemovePlayerDepthChartCommandHandler>>>();
    }

    [Theory, AutoData]
    public async Task Handle_RemovePlayerDepthChartCommand_WithValidQuery_ReturnsExpectedResults(
        RemovePlayerDepthChartCommand expectedCommand,
        List<Player> expectedResponse
        )
    {
        //arrange
        _mockPlayerRepository
            .Setup(c => c.RemovePlayerFromDepthChart(expectedCommand.Position, expectedCommand.Player))
            .Returns(expectedResponse);

        var handler = new RemovePlayerDepthChartCommandHandler(_mockPlayerRepository.Object, _mockLogger.Object);

        //act
        var result = await handler.Handle(expectedCommand, CancellationToken.None);

        //assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Theory, AutoData]
    public async Task Handle_RemovePlayerDepthChartCommand_WithException_ReturnsEmptyListResult(
        RemovePlayerDepthChartCommand expectedCommand,
        Exception expectedException
        )
    {
        //arrange
        _mockPlayerRepository
            .Setup(c => c.RemovePlayerFromDepthChart(expectedCommand.Position, expectedCommand.Player))
            .Throws(expectedException);

        var handler = new RemovePlayerDepthChartCommandHandler(_mockPlayerRepository.Object, _mockLogger.Object);

        //act
        var result = await handler.Handle(expectedCommand, CancellationToken.None);

        //assert
        result.Should().BeEmpty();
    }

    [Theory, AutoData]
    public async Task Handle_RemovePlayerDepthChartCommand_WithException_LogsCorrectly(
        RemovePlayerDepthChartCommand expectedCommand,
        Exception expectedException
        )
    {
        //arrange
        var expectedMessage = $"Failed to RemovePlayerDepthChartCommand for Player Number: {expectedCommand.Player?.Number}, Player Name: {expectedCommand.Player?.Name}";

        _mockPlayerRepository
            .Setup(c => c.RemovePlayerFromDepthChart(expectedCommand.Position, expectedCommand.Player))
            .Throws(expectedException);

        var handler = new RemovePlayerDepthChartCommandHandler(_mockPlayerRepository.Object, _mockLogger.Object);

        //act
        await handler.Handle(expectedCommand, CancellationToken.None);

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