using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FanDuelSolution.Application.NFL.Models;
using FanDuelSolution.Infrastructure.InMemory;
using FluentAssertions;
using System.Numerics;
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
        Player expectedPlayer
        )
    {
        //arrange
        var rut = new PlayerRepository();

        //act
        var act = () => rut.RemovePlayerFromDepthChart(expectedPlayer);

        //assert
        act.Should().Throw<Exception>().WithMessage($"Position: {expectedPlayer.Position} for player removal does not exist in the depth chart");
    }

    [Theory, AutoData]
    public void RemovePlayerFromDepthChart_PlayerNotFound_ReturnsEmptyList(
        Player expectedMissingPlayer,
        Player expectedPlayer,
        int expectedPlayerPositionDepth
        )
    {
        //arrange
        var rut = new PlayerRepository();
        rut.AddPlayerToDepthChart(expectedPlayer, expectedPlayerPositionDepth);

        //act
        var result = rut.RemovePlayerFromDepthChart(expectedMissingPlayer);

        //assert
        result.Should().BeEmpty();
    }

    [Theory, AutoData]
    public void RemovePlayerFromDepthChart_PlayerFound_ReturnsExpectedPlayer(
        Player expectedPlayer,
        int expectedPlayerPositionDepth
        )
    {
        //arrange
        var expectedPlayerList = new List<Player>() { expectedPlayer };

        var rut = new PlayerRepository();
        rut.AddPlayerToDepthChart(expectedPlayer, expectedPlayerPositionDepth);

        //act
        var result = rut.RemovePlayerFromDepthChart(expectedPlayer);

        //assert
        result.Should().BeEquivalentTo(expectedPlayerList);
    }

    [Theory, AutoData]
    public void GetBackups_WhenPositionNotFound_ThrowsException(
        Player expectedPlayer
        )
    {
        //arrange
        var rut = new PlayerRepository();

        //act
        var act = () => rut.GetBackups(expectedPlayer);

        //assert
        act.Should().Throw<Exception>().WithMessage($"Position backsups do not exist for given Position: {expectedPlayer.Position}");
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
        rut.AddPlayerToDepthChart(expectedPlayer, expectedPlayerPositionDepth);

        //act
        var result = rut.GetBackups(expectedPlayer);

        //assert
        result.Should().BeEmpty();
    }

    [Theory, AutoData]
    public void GetBackups_WhenFiveBacksupsExist_WithPositionDepthThree_ReturnsExpectedBackupListOfTwo(
        Player expectedPlayerOne,
        Player expectedPlayerTwo,
        Player expectedPlayerThree,
        Player expectedPlayerFour,
        Player expectedPlayer
        )
    {
        //arrange
        var rut = new PlayerRepository();
        rut.AddPlayerToDepthChart(expectedPlayerOne, null);
        rut.AddPlayerToDepthChart(expectedPlayerTwo, null);
        rut.AddPlayerToDepthChart(expectedPlayerThree, null);
        rut.AddPlayerToDepthChart(expectedPlayerFour, null);
        rut.AddPlayerToDepthChart(expectedPlayer, 3);

        //act
        var result = rut.GetBackups(expectedPlayer);

        //assert
        result.Count().Should().Be(2);
    }

    [Theory, AutoData]
    public void AddPlayerToDepthChart_WhenPositionDoesNotExist_AddsPlayerToDepthChart(
        Player expectedPlayer,
        int expectedPlayerPositionDepth
        )
    {
        //arrange
        var rut = new PlayerRepository();

        //act
        rut.AddPlayerToDepthChart(expectedPlayer, expectedPlayerPositionDepth);

        //assert
        rut.DepthChartNFL
            .Should()
            .ContainKey(expectedPlayer.Position)
            .WhoseValue
            .Should()
            .Contain(expectedPlayer);
    }

    [Theory, AutoData]
    public void AddPlayerToDepthChart_NoPositionDepthGiven_AddsPlayerToEnd(
        Player expectedPlayer,
        Player expectedPlayerOne,
        Player expectedPlayerTwo,
        Player expectedPlayerThree
        )
    {
        //arrange
        var rut = new PlayerRepository();

        rut.AddPlayerToDepthChart(expectedPlayerOne, null);
        rut.AddPlayerToDepthChart(expectedPlayerTwo, null);
        rut.AddPlayerToDepthChart(expectedPlayerThree, null);

        var expectedPlayerPositionDepth = 0;

        //act
        rut.AddPlayerToDepthChart(expectedPlayer, expectedPlayerPositionDepth);

        var result = rut.DepthChartNFL[expectedPlayer.Position];

        //assert
        result.Last().Should().Be(expectedPlayer);
    }

    [Theory, AutoData]
    public void AddPlayerToDepthChart_PositionDepthLast_AddsPlayerToEnd(
        Player expectedPlayer,
        Player expectedPlayerOne,
        Player expectedPlayerTwo,
        Player expectedPlayerThree
        )
    {
        //arrange
        var rut = new PlayerRepository();

        rut.AddPlayerToDepthChart(expectedPlayerOne, null);
        rut.AddPlayerToDepthChart(expectedPlayerTwo, 1);
        rut.AddPlayerToDepthChart(expectedPlayerThree, 2);

        var expectedPlayerPositionDepth = 4;

        //act
        rut.AddPlayerToDepthChart(expectedPlayer, expectedPlayerPositionDepth);

        var result = rut.DepthChartNFL[expectedPlayer.Position];

        //assert
        result.Last().Should().Be(expectedPlayer);
    }

    [Theory, AutoData]
    public void AddPlayerToDepthChart_WithValidPositionDepth_InsertsPlayerAtCorrectPosition(
        Player expectedPlayer,
        Player expectedPlayerOne,
        Player expectedPlayerTwo,
        Player expectedPlayerThree
        )
    {
        //arrange
        var rut = new PlayerRepository();

        rut.AddPlayerToDepthChart(expectedPlayerOne, null);
        rut.AddPlayerToDepthChart(expectedPlayerTwo, null);
        rut.AddPlayerToDepthChart(expectedPlayerThree, null);

        var expectedPlayerPositionDepth = 1;

        //act
        rut.AddPlayerToDepthChart(expectedPlayer, expectedPlayerPositionDepth);

        var result = rut.DepthChartNFL[expectedPlayer.Position];

        //assert
        result.First().Should().Be(expectedPlayer);
    }

    [Theory, AutoData]
    public void AddPlayerToDepthChart_PlayerAlreadyExistsInPosition_ThrowsException(Player expectedPlayer
        )
    {
        //arrange
        var rut = new PlayerRepository();

        rut.AddPlayerToDepthChart(expectedPlayer, null);

        //act
        var act = () => rut.AddPlayerToDepthChart(expectedPlayer, null);

        //assert
        act.Should().Throw<Exception>().WithMessage($"Player Number: {expectedPlayer.Number}, Player Name: {expectedPlayer.Name} already exists in Depth Chart For Position {expectedPlayer.Position}");
    }
}