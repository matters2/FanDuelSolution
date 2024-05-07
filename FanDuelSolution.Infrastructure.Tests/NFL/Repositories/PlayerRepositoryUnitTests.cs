using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FanDuelSolution.Application.NFL.Models;
using FanDuelSolution.Infrastructure.InMemory;
using FluentAssertions;
using Xunit;

namespace FanDuelSolution.Infrastructure.Tests.NFL.Repositories;

public class PlayerRepositoryUnitTests
{
    private readonly IFixture _fixture;

    public PlayerRepositoryUnitTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
    }

    [Fact]
    public void GetFullDepthChart_WithEmptyDepthChart_ReturnsEmptyMessage()
    {
        //arrange
        var rut = new PlayerRepository();

        //act
        var result = rut.GetFullDepthChart();

        //assert
        result.Should().Be("No results included in NFL Depth Chart");
    }

    [Theory, AutoData]
    public void RemovePlayerFromDepthChart_WhenPositionNotFound_ThrowsException(
        string expectedPosition,
        Player expectedPlayer
        )
    {
        //arrange
        var rut = new PlayerRepository();

        //act
        var act = () => rut.RemovePlayerFromDepthChart(expectedPosition, expectedPlayer);

        //assert
        act.Should().Throw<Exception>().WithMessage($"Position: {expectedPosition} for player removal does not exist in the depth chart");
    }

    [Theory, AutoData]
    public void RemovePlayerFromDepthChart_PlayerNotFound_ReturnsEmptyList(
        string expectedPosition,
        Player expectedMissingPlayer,
        Player expectedPlayer,
        int expectedPlayerPositionDepth
        )
    {
        //arrange
        var rut = new PlayerRepository();
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayer, expectedPlayerPositionDepth);

        //act
        var result = rut.RemovePlayerFromDepthChart(expectedPosition, expectedMissingPlayer);

        //assert
        result.Should().BeEmpty();
    }

    [Theory, AutoData]
    public void RemovePlayerFromDepthChart_PlayerFound_ReturnsExpectedPlayer(
        string expectedPosition,
        Player expectedPlayer,
        int expectedPlayerPositionDepth
        )
    {
        //arrange
        var expectedPlayerList = new List<Player>() { expectedPlayer };

        var rut = new PlayerRepository();
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayer, expectedPlayerPositionDepth);

        //act
        var result = rut.RemovePlayerFromDepthChart(expectedPosition, expectedPlayer);

        //assert
        result.Should().BeEquivalentTo(expectedPlayerList);
    }

    [Theory, AutoData]
    public void GetBackups_WhenPositionNotFound_ThrowsException(
        string expectedPosition,
        Player expectedPlayer
        )
    {
        //arrange
        var rut = new PlayerRepository();

        //act
        var act = () => rut.GetBackups(expectedPosition, expectedPlayer);

        //assert
        act.Should().Throw<Exception>().WithMessage($"Position backsups do not exist for Position: {expectedPosition}");
    }

    [Theory, AutoData]
    public void GetBackups_WhenNoBacksupsExist_ReturnsEmptyList(
        string expectedPosition,
        Player expectedPlayer,
        int expectedPlayerPositionDepth
        )
    {
        //arrange
        var rut = new PlayerRepository();
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayer, expectedPlayerPositionDepth);

        //act
        var result = rut.GetBackups(expectedPosition, expectedPlayer);

        //assert
        result.Should().BeEmpty();
    }

    [Theory, AutoData]
    public void GetBackups_WhenFiveBacksupsExist_WithPositionDepthThree_ReturnsExpectedBackupListOfTwo(
        string expectedPosition,
        Player expectedPlayerOne,
        Player expectedPlayerTwo,
        Player expectedPlayerThree,
        Player expectedPlayerFour,
        Player expectedPlayer
        )
    {
        //arrange
        var rut = new PlayerRepository();
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayerOne, null);
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayerTwo, null);
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayerThree, null);
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayerFour, null);
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayer, 3);

        //act
        var result = rut.GetBackups(expectedPosition, expectedPlayer);

        //assert
        result.Count().Should().Be(2);
    }

    [Theory, AutoData]
    public void AddPlayerToDepthChart_WhenPositionDoesNotExist_AddsPlayerToEnd(
        string expectedPosition,
        Player expectedPlayer,
        int expectedPlayerPositionDepth
        )
    {
        //arrange
        var rut = new PlayerRepository();

        //act
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayer, expectedPlayerPositionDepth);

        //assert
        rut.DepthChartNFL
            .Should()
            .ContainKey(expectedPosition)
            .WhoseValue
            .Should()
            .Contain(expectedPlayer);
    }

    [Theory, AutoData]
    public void AddPlayerToDepthChart_NoPositionDepthGiven_AddsPlayerToEnd(
        string expectedPosition,
        Player expectedPlayer,
        Player expectedPlayerOne,
        Player expectedPlayerTwo,
        Player expectedPlayerThree
        )
    {
        //arrange
        var rut = new PlayerRepository();

        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayerOne, null);
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayerTwo, null);
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayerThree, null);

        var expectedPlayerPositionDepth = 0;

        //act
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayer, expectedPlayerPositionDepth);

        var result = rut.DepthChartNFL[expectedPosition];

        //assert
        result.Last().Should().Be(expectedPlayer);
    }

    [Theory, AutoData]
    public void AddPlayerToDepthChart_PositionDepthLast_AddsPlayerToEnd(
        string expectedPosition,
        Player expectedPlayer,
        Player expectedPlayerOne,
        Player expectedPlayerTwo,
        Player expectedPlayerThree
        )
    {
        //arrange
        var rut = new PlayerRepository();

        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayerOne, null);
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayerTwo, 1);
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayerThree, 2);

        var expectedPlayerPositionDepth = 4;

        //act
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayer, expectedPlayerPositionDepth);

        var result = rut.DepthChartNFL[expectedPosition];

        //assert
        result.Last().Should().Be(expectedPlayer);
    }

    [Theory, AutoData]
    public void AddPlayerToDepthChart_WithValidPositionDepth_InsertsPlayerAtCorrectPosition(
        string expectedPosition,
        Player expectedPlayer,
        Player expectedPlayerOne,
        Player expectedPlayerTwo,
        Player expectedPlayerThree
        )
    {
        //arrange
        var rut = new PlayerRepository();

        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayerOne, null);
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayerTwo, null);
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayerThree, null);

        var expectedPlayerPositionDepth = 1;

        //act
        rut.AddPlayerToDepthChart(expectedPosition, expectedPlayer, expectedPlayerPositionDepth);

        var result = rut.DepthChartNFL[expectedPosition];

        //assert
        result.First().Should().Be(expectedPlayer);
    }
}