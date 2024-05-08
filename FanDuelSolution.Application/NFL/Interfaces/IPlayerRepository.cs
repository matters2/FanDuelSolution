using FanDuelSolution.Application.NFL.Models;

namespace FanDuelSolution.Application.Interfaces;

public interface IPlayerRepository
{
    public string GetFullDepthChart();
    public List<Player> GetBackups(Player player);
    public void AddPlayerToDepthChart(Player player, int? positionDepth);
    public List<Player> RemovePlayerFromDepthChart(Player player);
}