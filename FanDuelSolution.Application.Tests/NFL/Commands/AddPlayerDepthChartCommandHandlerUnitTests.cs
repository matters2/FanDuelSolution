using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FanDuelSolution.Application.Interfaces;
using FanDuelSolution.Application.NFL.Commands;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FanDuelSolution.Application.Tests.NFL.Commands
{
    public class AddPlayerDepthChartCommandHandlerUnitTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IPlayerRepository> _mockPlayerRepository;
        private readonly Mock<ILogger<AddPlayerToDepthChartCommandHandler>> _mockLogger;

        public AddPlayerDepthChartCommandHandlerUnitTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _mockPlayerRepository = _fixture.Freeze<Mock<IPlayerRepository>>();
            _mockLogger = _fixture.Freeze<Mock<ILogger<AddPlayerToDepthChartCommandHandler>>>();
        }

        [Theory, AutoData]
        public async Task Handle_AddPlayerToDepthChartCommand_WithValidCommand_ReturnsTrueResult(AddPlayerToDepthChartCommand expectedCommand)
        {
            //arrange
            var handler = new AddPlayerToDepthChartCommandHandler(_mockPlayerRepository.Object, _mockLogger.Object);

            //act
            var result = await handler.Handle(expectedCommand, CancellationToken.None);

            //assert
            result.Should().BeTrue();
        }

        [Theory, AutoData]
        public async Task Handle_AddPlayerToDepthChartCommand_WithException_ReturnsFalseResult(
            AddPlayerToDepthChartCommand expectedCommand,
            Exception expectedException
            )
        {
            //arrange
            _mockPlayerRepository
                .Setup(c => c.AddPlayerToDepthChart(expectedCommand.Position, expectedCommand.Player, expectedCommand.PositionDepth))
                .Throws(expectedException);

            var handler = new AddPlayerToDepthChartCommandHandler(_mockPlayerRepository.Object, _mockLogger.Object);

            //act
            var result = await handler.Handle(expectedCommand, CancellationToken.None);

            //assert
            result.Should().BeFalse();
        }

        [Theory, AutoData]
        public async Task Handle_AddPlayerToDepthChartCommand_WithException_LogsCorrectly(
            AddPlayerToDepthChartCommand expectedCommand,
            Exception expectedException
            )
        {
            //arrange
            var expectedMessage = $"Failed to AddPlayerToDepthChart for Player Number: {expectedCommand.Player.Number}, Player Name: {expectedCommand.Player?.Name}";

            _mockPlayerRepository
                .Setup(c => c.AddPlayerToDepthChart(expectedCommand.Position, expectedCommand.Player, expectedCommand.PositionDepth))
                .Throws(expectedException);

            var handler = new AddPlayerToDepthChartCommandHandler(_mockPlayerRepository.Object, _mockLogger.Object);

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
}