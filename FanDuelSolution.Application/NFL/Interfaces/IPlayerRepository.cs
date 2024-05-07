using FanDuelSolution.Application.NFL.Models;

namespace FanDuelSolution.Application.Interfaces;

public interface IPlayerRepository
{
    public string GetFullDepthChart();
    public List<Player> GetBackups(string position, Player player);
    public void AddPlayerToDepthChart(string position, Player player, int? positionDepth);
    public List<Player> RemovePlayerFromDepthChart(string position, Player player);
}