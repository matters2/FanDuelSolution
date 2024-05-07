using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FanDuelSolution.Application.Interfaces;
using FanDuelSolution.Application.NFL.Queries;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FanDuelSolution.Application.Tests.NFL.Queries;

public class GetFullDepthChartQueryHandlerUnitTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IPlayerRepository> _mockPlayerRepository;
    private readonly Mock<ILogger<GetFullDepthChartQueryHandler>> _mockLogger;

    public GetFullDepthChartQueryHandlerUnitTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _mockPlayerRepository = _fixture.Freeze<Mock<IPlayerRepository>>();
        _mockLogger = _fixture.Freeze<Mock<ILogger<GetFullDepthChartQueryHandler>>>();
    }

    [Theory, AutoData]
    public async Task Handle_GetFullDepthChartQuery_WithValidQuery_ReturnsExpectedResults(
        GetFullDepthChartQuery expectedQuery,
        string expectedResponse
        )
    {
        //arrange
        _mockPlayerRepository
            .Setup(c => c.GetFullDepthChart())
            .Returns(expectedResponse);

        var handler = new GetFullDepthChartQueryHandler(_mockPlayerRepository.Object, _mockLogger.Object);

        //act
        var result = await handler.Handle(expectedQuery, CancellationToken.None);

        //assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Theory, AutoData]
    public async Task Handle_GetFullDepthChartQuery_WithException_ReturnsEmptyString(
        GetFullDepthChartQuery expectedQuery,
        Exception expectedException
        )
    {
        //arrange
        _mockPlayerRepository
            .Setup(c => c.GetFullDepthChart())
            .Throws(expectedException);

        var handler = new GetFullDepthChartQueryHandler(_mockPlayerRepository.Object, _mockLogger.Object);

        //act
        var result = await handler.Handle(expectedQuery, CancellationToken.None);

        //assert
        result.Should().BeEmpty();
    }

    [Theory, AutoData]
    public async Task Handle_GetFullDepthChartQuery_WithException_LogsCorrectly(
        GetFullDepthChartQuery expectedQuery,
        Exception expectedException
        )
    {
        //arrange
        var expectedMessage = "Failed to handle GetFullDepthChartQuery";

        _mockPlayerRepository
            .Setup(c => c.GetFullDepthChart())
            .Throws(expectedException);

        var handler = new GetFullDepthChartQueryHandler(_mockPlayerRepository.Object, _mockLogger.Object);

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